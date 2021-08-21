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
    [SerializeField] public EffectsManager effectsManager;
    [SerializeField] playerMovwe playerScript;
    [SerializeField] LevelManager levelManager;
    [SerializeField] MapEffects mapEffector;

    [Header("Design Settings")]
    [SerializeField] Color mainColor;
    [SerializeField] Color secondColor;
    [SerializeField] public Color coreColor;

    [Header("Dash values")]
    [SerializeField] float dashSpeed;
    [SerializeField] float preDashDuration;
    [SerializeField] float preDashZoomMagnitude;
    [SerializeField] float realDashDuration;
    [SerializeField] float dashSlowDownDuration;

    [Header("Boom values")]
    [SerializeField] float boomZoom;
    [SerializeField] float boomWaitInAirDuration;

    float currentDashSpeed;

    public void OnDead()
    {
        effectsManager.StartParticle(effectsManager.dieParticle);
        cameraEffector.Shake(0.75f, 0.4f);
        GetComponent<SpriteRenderer>().enabled = false;
        mapEffector.OnDead();
    }
    private void Awake()
    {
        effectsManager.InitColors(mainColor, secondColor, coreColor);
    }
    public void TurnWalkParticle(bool on)
    {
        if (on)
            effectsManager.StartParticle(effectsManager.walkParticle);
        else
            effectsManager.walkParticle.gameObject.SetActive(false);
    }
    public void OnTurn(bool isUp)
    {
        if (isUp)
            effectsManager.StartParticle(effectsManager.turnParticleUp);
        else
            effectsManager.StartParticle(effectsManager.turnParticleDown);
    }
    public void StartDash()
    {
        StartCoroutine(DashEffect());
    }
    IEnumerator DashEffect()
    {
        //slow motion + camera zoom in (exp)
        currentDashSpeed = 0f;
        cameraEffector.SetFollow(transform.position * 0.7f);
        effectsManager.StartParticle(effectsManager.preDashParticle);
        StartCoroutine(ZoomCamera(preDashZoomMagnitude, preDashDuration, CurveType.Exponential));
        float eTime = 0f;
        while (eTime < preDashDuration)
        {
            yield return null;
            float x = eTime / preDashDuration;
            Time.timeScale = Mathf.Lerp(1, 0.4f, 1 - Mathf.Pow((x - 1), 2));
            eTime += Time.unscaledDeltaTime;
        }
        playerScript.setDashCollider(true);
        effectsManager.StartParticle(effectsManager.dashParticle);
        effectsManager.StartParticle(effectsManager.dashBurstParticlePrimary);

        //dash + camera zoom out
        Time.timeScale = 1f;
        currentDashSpeed = dashSpeed;
        StartCoroutine(ZoomCamera(0, 0.2f, CurveType.Linear));
        cameraEffector.SetFollow(new Vector3(0, 0, 0));
        eTime = 0f;
        //update dash speed
        while (eTime < realDashDuration)
        {
            yield return null;
            eTime += Time.unscaledDeltaTime;
        }
        //end
        currentDashSpeed = 1;
    }
    public void StartBoom()
    {
        StartCoroutine(BoomCoroutine());
    }
    IEnumerator BoomCoroutine()
    {
        float eTime = 0f;
        cameraEffector.SetFollow(transform.position * 1f);
        StartCoroutine(ZoomCamera(boomZoom, playerScript.chargeDuration, CurveType.Exponential));
        while (eTime < playerScript.chargeDuration - boomWaitInAirDuration)
        {
            yield return null;
            eTime += Time.unscaledDeltaTime;
        }
        eTime = 0f;
        Time.timeScale = 0.8f;
        while(eTime < boomWaitInAirDuration)
        {
            yield return null;
            eTime += Time.unscaledDeltaTime;
        }
        Time.timeScale = 1f;
        cameraEffector.SetFollow(new Vector3(0, 0, 0));
        StartCoroutine(ZoomCamera(0, 0.1f, CurveType.Linear));
    }
    public void OnBoom()
    {
        StartCoroutine(OnBoomCoroutine());
    }
    IEnumerator OnBoomCoroutine()
    {
        effectsManager.StartParticle(effectsManager.boomCircleParticle);
        cameraEffector.Shake(0.7f, 0.5f);
        yield return null;
    }

    IEnumerator ZoomCamera(float zoomTarget, float duration, CurveType curveType)
    {
        float eTime = 0f;
        float originalSize = cameraEffector.CurrentZoom;
        while (eTime < duration)
        {
            yield return null;
            float x = eTime / duration;
            if (curveType == CurveType.Exponential)
            {
                cameraEffector.SetZoom(Mathf.Lerp(originalSize, zoomTarget, 1 - Mathf.Pow((x - 1), 2)));
            }
            else
            {
                cameraEffector.SetZoom(Mathf.Lerp(originalSize, zoomTarget, x));

            }
            eTime += Time.unscaledDeltaTime;
        }
        cameraEffector.SetZoom(zoomTarget);
    }
    public void OnBlockBreak()
    {
        GameObject g = Instantiate(effectsManager.blockDestroyPrefab,transform.position, Quaternion.identity);
        g.SetActive(true);
        cameraEffector.Shake();
        levelManager.SetScore(levelManager.Score + levelManager.NormalWallScore);
    }
    public float GetDashSpeed()
    {
        return currentDashSpeed;
    }
}
