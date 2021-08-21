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

    [Header("Effect Values")]
    [SerializeField] float InitialZoomOutDuration;
    [SerializeField] float dieWaitDuration;
    [SerializeField] float dieWhiteCoverDuration;
    [SerializeField] float dieWhiteCoverScale;
    [SerializeField] float gameClearDestroyDelay;

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
    }
    public void OnGameClear()
    {
        player.GameClear();
        blockSpawner.DeActivate();
        StartCoroutine(GameClearCoroutine());
    }
    IEnumerator GameClearCoroutine()
    {
        yield return new WaitForSeconds(gameClearDestroyDelay);
        var blocks = GameObject.FindGameObjectsWithTag("block");
        cameraEffector.Shake(0.6f, blocks.Length*0.1f);
        foreach(GameObject go in blocks)
        {
            go.SetActive(false);
            player.effector.OnBlockBreak();

            GameObject g = Instantiate(player.effector.effectsManager.blockDestroyPrefab, go.transform.position, Quaternion.identity);
            g.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }

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
