using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject blockObject;
    public playerMovwe p;

    public float fillSpeed = 1;

    float scaleFactor;
    float toScaleFactor;
    Vector2 normalScale;
    float eTime =  10000;



    [Header("SpawnValues")]
    public int SpawnCount = 10;
    public float TimeOfSpawn = 0.5f;
    public float ShrinkDuration = 0.5f;

    void Awake()
    {
        
    }
    private void OnEnable()
    {
        scaleFactor = 1;
        normalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {//이렇게 해도 되고 FSM으로 idle일땐 차오르고 꽝찍어서 쪼그라드는 STATE도 있고 그렇게 해도 되고 그건 나중에..

            if (eTime <= ShrinkDuration)
            {
                eTime += Time.unscaledDeltaTime;
                float x;
                x = Mathf.Lerp(scaleFactor, toScaleFactor, eTime / ShrinkDuration);
                transform.localScale = normalScale * x;
                if (eTime >= ShrinkDuration)
                {
                    scaleFactor = x;
                }
            }
            else if(!p.isBoom())
            {
                scaleFactor += Time.deltaTime * fillSpeed;
                transform.localScale = normalScale * scaleFactor;
            }
        if (!p.isDie()&&!p.isBoom()&&Input.GetKeyDown(KeyCode.Q))
        {
            p.setBoom(this);
        }
        
    }
    public void cutCenter(float scaleF = 0.5f)
    {
        toScaleFactor = scaleF;
        eTime = 0;
    }
    public void SpawnBlocks(int count = 10,float timeOfSpawn = 0.5f)
    {
        float RangeMin = p.rangeMin;
        float RangeMax = p.rangeMax;
        for (int i = 0; i < count; i++)
        {
            var g = Instantiate(blockObject);
            g.transform.parent = transform.parent;
            g.transform.localPosition = Vector2.zero;
            float theta = Random.Range(0,Mathf.PI*2);
            float Range = Random.Range(RangeMin, RangeMax);
            g.GetComponent<blockBase>().setDest(theta,Range,timeOfSpawn,p);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            p.centerCollisionEnter(this);
        }
    }

}
