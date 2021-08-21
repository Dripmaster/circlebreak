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
    public ParticleSystem walkParticle;
    public ParticleSystem dieParticle;
    public ParticleSystem dieParticle1;
    public ParticleSystem dieParticle2;

    public ParticleSystem boomCircleParticle;
    public ParticleSystem boomCircleParticle1;
    public ParticleSystem boomCircleParticle2;

    public void StartParticle(ParticleSystem particle)
    {
        particle.gameObject.SetActive(false);
        particle.gameObject.SetActive(true);
    }
    public void InitColors(Color mainColor, Color secondColor, Color coreColor)
    {
        SetColor(dashParticle, mainColor);
        SetColor(dashBurstParticlePrimary, mainColor);
        SetColor(dashBurstParticleSecondary, secondColor);
        SetColor(turnParticleDown, mainColor);
        SetColor(turnParticleUp, mainColor);
        SetColor(walkParticle, mainColor);
        SetColor(dieParticle, mainColor);
        SetColor(dieParticle1, mainColor);
        SetColor(dieParticle2, secondColor);
        SetColor(boomCircleParticle, coreColor);
        SetColor(boomCircleParticle1, coreColor);
        SetColor(boomCircleParticle2, coreColor);
    }
    void SetColor(ParticleSystem particle, Color color)
    {
        var main = particle.main;
        main.startColor = color;
    }
}
