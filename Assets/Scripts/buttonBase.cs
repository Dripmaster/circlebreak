using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonBase : wallBase
{
    public virtual void buttonAction() { Debug.Log("버튼눌림:" + name); }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!_playerMove.isFever() && !isBreak && !isSpawning)
            {
                buttonAction();
            }
            else
            if (_playerMove.isFever())
            {
            }
        }
    }
}
