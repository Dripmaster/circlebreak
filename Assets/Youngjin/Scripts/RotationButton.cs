using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationButton : buttonBase
{
    [SerializeField] GameObject pressParticle;
    public override void buttonAction()
    {
        SoundManager.Singleton.PlaySound(SoundManager.Singleton.trapSound);
        pressParticle.SetActive(true);
        Camera.main.GetComponent<CameraEffector>().Shake(0.25f);
        GameObject.FindGameObjectWithTag("MapManager").GetComponent<ClubMapManager>().OnRotationButton();

        gameObject.SetActive(false);
    }
}
