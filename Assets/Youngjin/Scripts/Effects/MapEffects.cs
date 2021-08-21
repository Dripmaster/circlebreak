using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEffects : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CameraEffector cameraEffector;
    [SerializeField] Canvas canvas;
    [SerializeField] playerMovwe player;
    [SerializeField] SpriteRenderer whiteCover;
    [SerializeField] LevelManager levelManager;

    [Header("Effect Values")]
    [SerializeField] float InitialZoomOutDuration;
    [SerializeField] float dieWaitDuration;
    [SerializeField] float dieWhiteCoverDuration;
    [SerializeField] float dieWhiteCoverScale;

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
    }
    public void OnDead()
    {
        StartCoroutine(Die());
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
