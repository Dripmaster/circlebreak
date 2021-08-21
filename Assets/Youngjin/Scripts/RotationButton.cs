using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationButton : buttonBase
{
    [SerializeField] GameObject pressParticle;
    public override void buttonAction() 
    {
        pressParticle.SetActive(true);
        GameObject.FindGameObjectWithTag("MapManager").GetComponent<ClubMapManager>().OnRotationButton();
    }
}
