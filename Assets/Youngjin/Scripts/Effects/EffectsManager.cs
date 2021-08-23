using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public GameObject blockDestroyPrefab;

    public ParticleSystem preDashParticle;
    public ParticleSystem dashParticle;
    public ParticleSystem dashCircle;
    public ParticleSystem dashBurstParticlePrimary;
    public ParticleSystem dashBurstParticleSecondary;
    public ParticleSystem turnParticleUp;
    public ParticleSystem turnParticleDown;
    public ParticleSystem walkParticle;
    public ParticleSystem dieParticle;
    public ParticleSystem dieParticle1;
    public ParticleSystem dieParticle2;

    public ParticleSystem boomCircleParticle;
    public ParticleSystem boomParticle1;
    public ParticleSystem boomParticle2;
    public ParticleSystem boomParticle3;
    public ParticleSystem boomParticle4;

    public ParticleSystem blockDestroyCircleParticle;
    public ParticleSystem blockDestroyParticle;
    public ParticleSystem feverParticle;

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
        SetColor(boomParticle1, coreColor);
        SetColor(boomParticle2, coreColor);
        SetColor(boomParticle3, mainColor);
        SetColor(boomParticle4, mainColor);
        SetColor(blockDestroyCircleParticle, coreColor);
        SetColor(blockDestroyParticle, coreColor);
        SetColor(feverParticle, mainColor);
    }
    void SetColor(ParticleSystem particle, Color color)
    {
        var main = particle.main;
        main.startColor = color;
    }
}
