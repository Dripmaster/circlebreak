using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public GameObject blockDestroyPrefab;

    public ParticleSystem preDashParticle;
    public ParticleSystem dashParticle;
    public ParticleSystem dashBurstParticlePrimary;
    public ParticleSystem dashBurstParticleSecondary;
    public ParticleSystem turnParticleUp;
    public ParticleSystem turnParticleDown;

    public ParticleSystem boomCircleParticle;

    public void StartParticle(ParticleSystem particle)
    {
        particle.gameObject.SetActive(false);
        particle.gameObject.SetActive(true);
    }
    public void InitColors(Color mainColor, Color secondColor)
    {
        SetColor(dashParticle, mainColor);
        SetColor(dashBurstParticlePrimary, mainColor);
        SetColor(dashBurstParticleSecondary, secondColor);
        SetColor(turnParticleDown, mainColor);
        SetColor(turnParticleUp, mainColor);

    }
    void SetColor(ParticleSystem particle, Color color)
    {
        var main = particle.main;
        main.startColor = color;
    }
}
