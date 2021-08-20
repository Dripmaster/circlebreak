using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    enum CurveType { Exponential, Linear }
    public delegate float EffectCurve(float time);

    public float FullDashDuration { get { return realDashDuration + preDashDuration; } }

    [Header("References")]
    [SerializeField] CameraEffector cameraEffector;
    [SerializeField] GameObject dashParticle;
    [SerializeField] GameObject dashBurstParticle;

    [Header("Dash values")]
    [SerializeField] float dashSpeed;
    [SerializeField] float preDashDuration;
    [SerializeField] float preDashZoomMagnitude;
    [SerializeField] float realDashDuration;
    [SerializeField] float dashSlowDownDuration;

    float currentDashSpeed;

    
    public void StartDash()
    {
        StartCoroutine(DashEffect());
    }
    IEnumerator DashEffect()
    {
        //slow motion + camera zoom in (exp)
        cameraEffector.SetFollow(transform.position / 2);
        StartCoroutine(ZoomCamera(preDashZoomMagnitude, preDashDuration, CurveType.Exponential));
        float eTime = 0f;
        while (eTime < preDashDuration)
        {
            yield return null;
            float x = eTime / preDashDuration;
            Time.timeScale = Mathf.Lerp(1, 0.4f, 1-Mathf.Pow((x - 1), 2));
            eTime += Time.unscaledDeltaTime;
        }
        dashBurstParticle.SetActive(true);
        dashParticle.SetActive(true);
        //dash + camera zoom out
        Time.timeScale = 1f;
        currentDashSpeed = dashSpeed;
        StartCoroutine(ZoomCamera(0, 0.2f, CurveType.Linear));
        cameraEffector.SetFollow(new Vector3(0,0,0));
        eTime = 0f; 

        //update dash speed
        while(eTime < realDashDuration)
        {
            yield return null;
            eTime += Time.unscaledDeltaTime;
        }
        //end
        currentDashSpeed = 1;
    }

    IEnumerator ZoomCamera(float zoomTarget, float duration, CurveType curveType)
    {
        float eTime = 0f;
        float originalSize = cameraEffector.CurrentZoom;
        while(eTime < duration)
        {
            yield return null;
            float x = eTime / duration;
            if (curveType == CurveType.Exponential)
            {
                cameraEffector.SetZoom(Mathf.Lerp(originalSize, zoomTarget, 1-Mathf.Pow((x - 1),2)));
            }
            else
            {
                cameraEffector.SetZoom(Mathf.Lerp(originalSize, zoomTarget, x));

            }
            eTime += Time.unscaledDeltaTime;
        }
        cameraEffector.SetZoom(zoomTarget);
    }

    public float GetDashSpeed()
    {
        return currentDashSpeed;
    }
}
