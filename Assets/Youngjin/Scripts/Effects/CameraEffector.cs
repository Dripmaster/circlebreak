using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffector : MonoBehaviour
{
    public float CurrentZoom { get { return cameraZoom; } }

    [SerializeField] float followSpeed;
    [SerializeField] float initialZoom;
    float originalSize;
    Vector3 originalPos;
    Vector3 targetPos;
    float cameraZoom = 0;
    Camera cameraComponent;


    private void Awake()
    {
        cameraComponent = GetComponent<Camera>();
        originalSize = cameraComponent.orthographicSize;
        SetZoom(initialZoom);
        originalPos = transform.parent.position;
    }
    public void SetZoom(float magnitude)
    {
        cameraComponent.orthographicSize = originalSize - magnitude;
        cameraZoom = magnitude;
    }
    public IEnumerator SetZoomCoroutine(float zoomTarget, float duration, TimeCurve curve)
    {
        float eTime = 0f;
        float originalSize = CurrentZoom;
        while (eTime < duration)
        {
            yield return null;
            float x = eTime / duration;
            SetZoom(Mathf.Lerp(originalSize, zoomTarget, curve(x)));
            eTime += Time.deltaTime;
        }
        SetZoom(zoomTarget);
    }
    public void SetFollow(Vector3 targetPosition)
    {
        targetPos = targetPosition;
    }
    private void Update()
    {
        {
            transform.parent.position = Vector3.Lerp(transform.parent.position, targetPos, Time.deltaTime * followSpeed);
        }
    }
    public void Shake(float magnitude = 0.5f, float duration = 0.35f)
    {
        StartCoroutine(VectorUtils.Shake(transform, magnitude, duration));
    }
}
