using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallBase : blockBase
{
    public int countOfDestroy = 2;
    public float destroyDuration = 0.3f;
    bool isBreak;
    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpawning)
        {
            eTime += Time.deltaTime;
            if (eTime >= timeOfSpawn)
            {
                eTime = timeOfSpawn;
                isSpawning = false;
                effector.OnLand();
            }
            Vector2 newDest = dest;
            newDest.x *= _playerMove.GetBigCircleRatio().x;
            newDest.y *= _playerMove.GetBigCircleRatio().y;
            newDest = Quaternion.Euler(0, 0, transform.parent.rotation.eulerAngles.z) * newDest;
            
            newDest += (Vector2)transform.parent.position;
            transform.position = Vector2.Lerp(transform.parent.position, newDest, eTime / timeOfSpawn);
        }
        if (isBreak)
        {
            eTime += Time.deltaTime;
            if (eTime >= destroyDuration)
            {
                gameObject.SetActive(false);
            }
        }
    }
    private void OnEnable()
    {
        isSpawning = false;
        isBreak = false;
        eTime = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!_playerMove.isFever() && !isBreak && !isSpawning)
            {
                _playerMove.wallCollisionEnter(this);
            }
            else
            if (_playerMove.isFever())
            {
                _playerMove.effector.OnBlockBreak();//OnWallBreak?
                gameObject.SetActive(false);
            }
        }
    }
    public void OnLineEnter(Collider2D collision)
    {
        if (!_playerMove.isDie()&& !isBreak && !isSpawning && collision.tag == "Player")
        {
            countOfDestroy -= 1;
            if (countOfDestroy <= 0)
            {
                isBreak = true;
                eTime = 0;
            }
        }
    }
}
