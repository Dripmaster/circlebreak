using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockBase : MonoBehaviour
{
    [SerializeField] protected BlockEffects effector;
    public playerMovwe _playerMove;
    protected Vector2 dest;
    protected float timeOfSpawn;
    protected float eTime;
    protected bool isSpawning;


    
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
    public void setDest(float theta, float Range,float _timeOfSpawn, playerMovwe p)
    {
        _playerMove = p;
        isSpawning = true;

        dest = Vector2.zero;

        dest.x = Range*Mathf.Cos(-theta);
        dest.y = Range*Mathf.Sin(-theta);

        transform.localRotation = Quaternion.Euler(0, 0,Mathf.Rad2Deg*-theta);

        timeOfSpawn = _timeOfSpawn;

        GetComponentInChildren<Animator>().SetFloat("Speed", 1 / _timeOfSpawn);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isSpawning&&collision.tag == "Player")
        {
            if(!_playerMove.isDashOrFever())
            {
                _playerMove.blockCollisionEnter(this);
            }
            if (_playerMove.isFever())
            {
                _playerMove.effector.OnBlockBreak();
                gameObject.SetActive(false);
            }
            if (_playerMove.isBoom())
            {
                _playerMove.effector.OnBlockBreak();
                gameObject.SetActive(false);
            }
        }
        if (!isSpawning && collision.tag == "dashCollider")
        {
            if (_playerMove.isDashOrFever())
            {
                _playerMove.effector.OnBlockBreak();
                gameObject.SetActive(false);
            }
        }
    }
}
