using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UturnButton : buttonBase
{
    [SerializeField] GameObject particle;
    public override void buttonAction()
    {
        SoundManager.Singleton.PlaySound(SoundManager.Singleton.trapSound);
        _playerMove.StartCoroutine(Uturn());
        Camera.main.GetComponent<CameraEffector>().Shake(0.25f);
        particle.transform.parent = transform.parent;
        particle.SetActive(true);
        isBreak = true;
    }
    IEnumerator Uturn()
    {
        int dir = _playerMove.FowardDir * -1;
        _playerMove.FowardDir = 0;
        yield return new WaitForSeconds(0.5f);
        _playerMove.FowardDir = dir;
    }
}
