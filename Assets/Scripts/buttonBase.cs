using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonBase : wallBase
{
    public virtual void buttonAction() { Debug.Log("버튼눌림:" + name); //isBreak = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!_playerMove.isFever() && !isBreak && !isSpawning)
            {
                buttonAction();
                SoundManager.Singleton.PlaySound(SoundManager.Singleton.trapSound);
                        //isBreak = true;
            }
            else
            if (_playerMove.isFever())
            {
            }
        }
    }
    private void OnDestroy()
    {
    }
}
