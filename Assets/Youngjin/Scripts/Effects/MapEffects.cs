using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEffects : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CameraEffector cameraEffector;
    [SerializeField] Canvas canvas;
    [SerializeField] public playerMovwe player;
    [SerializeField] SpriteRenderer whiteCover;
    [SerializeField] LevelManager levelManager;
    [SerializeField] public blockSpawner blockSpawner;
    [SerializeField] Transform background;
    [SerializeField] GameObject spawnerExplosionPrefab;
    [SerializeField] MapManager mapManager;

    [Header("Effect Values")]
    [SerializeField] float InitialZoomOutDuration;
    [SerializeField] float dieWaitDuration;
    [SerializeField] float dieWhiteCoverDuration;
    [SerializeField] float dieWhiteCoverScale;
    [SerializeField] float gameClearDestroyDelay;
    [SerializeField] float gameClearExplodeDuration;
    [SerializeField] float shakeInterval;

    private void Start()
    {
        StartCoroutine(SceneStart());
    }
    IEnumerator SceneStart()
    {
        yield return null;
        cameraEffector.Shake(0.2f, InitialZoomOutDuration * 0.7f);
        yield return StartCoroutine(cameraEffector.SetZoomCoroutine(0, InitialZoomOutDuration, TimeCurves.ExponentialPingPong));
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        player.ReadyDone();
        blockSpawner.Activate();
        mapManager.OnReadyDone();

    }
    public void OnGameClear()
    {
        player.GameClear();
        blockSpawner.DeActivate();
        var main = spawnerExplosionPrefab.GetComponent<ParticleSystem>().main;
        main.startColor = blockSpawner.MainColor;
        StartCoroutine(GameClearCoroutine());
    }
    IEnumerator GameClearCoroutine()
    {
        float eTime = 0f;
            Time.timeScale = 1;
        Vector3 bgInitialScale = background.localScale;
        float bgInitialRotation = background.rotation.eulerAngles.z;
        Vector3 bgInitialPosition = background.position;
        float initialZoom = cameraEffector.CurrentZoom;
        Vector3 initialCameraPos = cameraEffector.transform.parent.position;
        cameraEffector.SetFollow(new Vector3(0, 0, 0));
        while (eTime < gameClearDestroyDelay)
        {
            yield return null;
            eTime += Time.unscaledDeltaTime;
            float x = TimeCurves.ExponentialMirrored(eTime / gameClearDestroyDelay);
            background.localScale = Vector3.Lerp(bgInitialScale, new Vector3(1, 1, 1), x);
            background.position = Vector3.Lerp(bgInitialPosition, new Vector3(0,0,0), x);
            background.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(bgInitialRotation, 0, x)));
            cameraEffector.SetZoom(Mathf.Lerp(initialZoom, 0, x));
            cameraEffector.SetFollow(Vector3.Lerp(initialCameraPos, new Vector3(0,0,0),x));
        }
        var blocks = GameObject.FindGameObjectsWithTag("block");
        cameraEffector.Shake(0.6f, blocks.Length*0.13f);
        foreach(GameObject go in blocks)
        {
            go.SetActive(false);

            GameObject g = Instantiate(player.effector.effectsManager.blockDestroyPrefab, go.transform.position, Quaternion.identity);
            g.SetActive(true);
            yield return new WaitForSeconds(0.15f);
        }
        cameraEffector.Shake(0.1f, gameClearExplodeDuration);
        eTime = 0f;
        int shakeCount = 0;
        Vector3 originalPosition = player.transform.position;
        canvas.renderMode = RenderMode.WorldSpace;
        while (eTime < gameClearExplodeDuration)
        {
            yield return null;
            eTime += Time.unscaledDeltaTime;
            if(eTime > shakeCount * shakeInterval)
            {
                shakeCount++;
                StartCoroutine(VectorUtils.Shake(blockSpawner.transform, 0.2f, shakeInterval));
                Vector3 pos = new Vector3(Random.Range(0.4f, 0.8f) * (Random.Range(0,2)*2-1), Random.Range(0.4f, 0.8f) * (Random.Range(0, 2) * 2 - 1), 0) * blockSpawner.transform.localScale.x * 0.5f;
                GameObject go = Instantiate(spawnerExplosionPrefab, pos, Quaternion.identity);
                go.transform.localScale = blockSpawner.transform.localScale;
                //go.transform.parent = blockSpawner.transform;
                go.SetActive(true);
            }
            if (eTime > gameClearExplodeDuration/3)
            {
                float x = TimeCurves.Exponential((eTime-gameClearExplodeDuration/3) / (gameClearExplodeDuration*2/3));
                player.transform.position = Vector3.Lerp(originalPosition, new Vector3(originalPosition.x,
                    6.5f, 0), x);
                cameraEffector.SetFollow(new Vector3(0, Mathf.Lerp(0, 2, x), 0));
                cameraEffector.SetZoom(Mathf.Lerp(0, 1, x));
            }
        }
        eTime = 0;
        while (eTime < 1)
        {
            yield return null;
            eTime += Time.unscaledDeltaTime;
            float x = eTime / 1;
            whiteCover.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, x));
            whiteCover.transform.localScale = new Vector2(1, 1) * Mathf.Lerp(0, dieWhiteCoverScale, TimeCurves.ExponentialMirrored(x));
        }

        levelManager.ChangeScene();
    }
    public void OnDead()
    {
        StartCoroutine(Die());
        blockSpawner.DeActivate();
    }
    IEnumerator Die()
    {
        yield return new WaitForSeconds(dieWaitDuration);
        float eTime = 0f;
        while(eTime < dieWhiteCoverDuration)
        {
            yield return null;
            eTime += Time.unscaledDeltaTime;
            float x = eTime / dieWhiteCoverDuration;
            whiteCover.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, x));
            whiteCover.transform.localScale = new Vector2(1, 1) * Mathf.Lerp(0, dieWhiteCoverScale, TimeCurves.ExponentialMirrored(x));
        }
        levelManager.ChangeScene();
    }
}
