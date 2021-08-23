using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clockBtn : buttonBase
{
    [SerializeField] GameObject particle;
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
        Camera.main.GetComponent<CameraEffector>().Shake(0.25f);
        _playerMove.StartCoroutine(Uturn(mgr));
        particle.transform.parent = transform.parent;
        particle.SetActive(true);
        gameObject.SetActive(false);
        isBreak = true;
    }
    IEnumerator Uturn(TimeMapManager mgr)
    {
        int dir = _playerMove.FowardDir * -1;
        _playerMove.FowardDir = 0;
        yield return new WaitForSeconds(0.5f);
        _playerMove.FowardDir = dir;
        mgr.setTimesc(mgr.timesc() * -1);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!isBreak && !isSpawning)
            {
                SoundManager.Singleton.PlaySound(SoundManager.Singleton.trapSound);
                buttonAction();
                gameObject.SetActive(false);
                isBreak = true;
            }
        }
    }
}
