using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yoloScript : buttonBase
{

    [SerializeField] GameObject particle;
    bool isTriggerStart;
    bool isUsed;
    // Start is called before the first frame update
    private new void OnEnable()
    {
        base.OnEnable();
        isUsed = false;
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isUsed&&_playerMove.isDashOrFever()&& collision.tag == "Player")
        {
            isTriggerStart = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if ( collision.tag == "Player")
        {
            if (_playerMove.isDashOrFever())
            {
                if (isTriggerStart)
                {
                    Camera.main.GetComponent<CameraEffector>().Shake(0.4f);
                    particle.transform.parent = transform.parent;
                    particle.SetActive(true);
                    _playerMove.setFever();
                    isUsed = true;
                    isBreak = true;
                    gameObject.SetActive(false);
                }
            }

            isTriggerStart = false;
        }
    }
}
