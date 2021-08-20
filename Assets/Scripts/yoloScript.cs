using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yoloScript : MonoBehaviour
{

    bool isTriggerStart;
    bool isUsed;
    public playerMovwe player;
    // Start is called before the first frame update
    void Awake()
    {
    }
    private void OnEnable()
    {
        isUsed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isUsed&& player.isDashOrFever()&& collision.tag == "Player")
        {
            isTriggerStart = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if ( collision.tag == "Player")
        {
            if (player.isDashOrFever())
            {
                if (isTriggerStart)
                {
                    player.setFever();
                    isUsed = true;
                    gameObject.SetActive(false);
                }
            }

            isTriggerStart = false;
        }
    }
}
