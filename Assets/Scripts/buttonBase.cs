using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonBase : wallBase
{
    public virtual void buttonAction() { Debug.Log("��ư����:" + name); }
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
