using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationButton : buttonBase
{
    [SerializeField] GameObject pressParticle;
    public override void buttonAction() 
    {
        pressParticle.SetActive(true);
        Camera.main.GetComponent<CameraEffector>().Shake(0.25f);
        GameObject.FindGameObjectWithTag("MapManager").GetComponent<ClubMapManager>().OnRotationButton();

        isBreak = true;
    }
}
