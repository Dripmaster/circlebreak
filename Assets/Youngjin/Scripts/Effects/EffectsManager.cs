using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public ParticleSystem blockDestroyParticle;

    public ParticleSystem preDashParticle;
    public ParticleSystem dashParticle;
    public ParticleSystem dashBurstParticlePrimary;
    public ParticleSystem dashBurstParticleSecondary;

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

    }
    void SetColor(ParticleSystem particle, Color color)
    {
        var main = particle.main;
        main.startColor = color;
    }
}
