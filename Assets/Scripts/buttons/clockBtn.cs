using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clockBtn : buttonBase
{
    TimeMapManager mgr;
    private void Awake()
    {
        mgr = GameObject.Find("TimeMapManager").GetComponent<TimeMapManager>();
    }
    private void Update()
    {
        base.Update();
        transform.Rotate(0,0,Time.deltaTime*8);
    }
    public override void buttonAction()
    {
        _playerMove.FowardDir *= -1;
        isBreak = true;
        mgr.setTimesc(mgr.timesc()*-1);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!isBreak && !isSpawning)
            {
                buttonAction();
                isBreak = true;
            }
        }
    }
}
