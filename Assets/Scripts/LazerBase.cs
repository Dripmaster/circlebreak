using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerBase : MonoBehaviour
{
    public playerMovwe _playerMove;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player") {
            if(!_playerMove.isDashOrFever()) {
                _playerMove.blockCollisionEnter(new blockBase());
            }
        }
        if(collision.tag == "dashCollider") {
            if (_playerMove.isDashOrFever()) {
                _playerMove.effector.OnBlockBreak();
            }
        }
    }
}
