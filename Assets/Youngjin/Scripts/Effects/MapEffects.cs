using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEffects : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CameraEffector cameraEffector;

    [Header("Effect Values")]
    [SerializeField] float InitialZoomOutDuration;

    private void Start()
    {
        StartCoroutine(SceneStart());
    }
    IEnumerator SceneStart()
    {
        yield return null;
        //cameraEffector.Shake(0.4f, 0.7f);
        StartCoroutine(cameraEffector.SetZoomCoroutine(0, InitialZoomOutDuration, TimeCurves.Exponential));
    }
}
