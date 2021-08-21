using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEffects : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CameraEffector cameraEffector;
    [SerializeField] Canvas canvas;

    [Header("Effect Values")]
    [SerializeField] float InitialZoomOutDuration;

    private void Start()
    {
        StartCoroutine(SceneStart());
    }
    IEnumerator SceneStart()
    {
        yield return null;
        cameraEffector.Shake(0.4f, InitialZoomOutDuration);
        yield return StartCoroutine(cameraEffector.SetZoomCoroutine(0, InitialZoomOutDuration, TimeCurves.Exponential));
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
    }
}
