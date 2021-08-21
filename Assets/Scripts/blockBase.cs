using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockBase : MonoBehaviour
{
    [SerializeField] BlockEffects effector;
    public playerMovwe _playerMove;

    Vector2 dest;
    float timeOfSpawn;
    float eTime;
    bool isSpawning;
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
    }
    private void OnEnable()
    {
        isSpawning = false;
        eTime = 0;
    }
    public void setDest(float theta, float Range,float _timeOfSpawn,playerMovwe p)
    {
        _playerMove = p;
        isSpawning = true;

        dest = Vector2.zero;

        dest.x = Range*Mathf.Cos(-theta);
        dest.y = Range*Mathf.Sin(-theta);

        transform.localRotation = Quaternion.Euler(0, 0,Mathf.Rad2Deg*-theta);

        timeOfSpawn = _timeOfSpawn;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isSpawning&&collision.tag == "Player")
        {
            if(_playerMove.isDashOrFever())
            {
                _playerMove.effector.OnBlockBreak();
                gameObject.SetActive(false);
            }
            else
            {
                _playerMove.blockCollisionEnter(this);
            }
        }
    }
}
