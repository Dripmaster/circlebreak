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



    [Header("MassiveSpawnValues")]
    public int SpawnCount = 10;
    public float TimeOfSpawn = 0.5f;
    public float ShrinkDuration = 0.5f;
    public float massiveSafetyZoneDegree = 90f;
    public int massiveSpawnDegreeDivideCount = 27;
    [Header("PassiveSpawnValues")]
    public float passiveSpawnTime = 1;
    public float passiveDangerZoneDegree= 45f;

    void Awake()
    {
        
    }
    private void OnEnable()
    {
        scaleFactor = 1;
        normalScale = transform.localScale;
        StartCoroutine(spawn());
    }

    IEnumerator spawn()
    {
        float eTime = 0;
        do
        {
            eTime += Time.unscaledDeltaTime;
            if (eTime >= passiveSpawnTime)
            {
                eTime = 0;
                SpawnBlock();
            }
            yield return null;
        } while (true);
    }

    // Update is called once per frame
    void Update()
    {//이렇게 해도 되고 FSM으로 idle일땐 차오르고 꽝찍어서 쪼그라드는 STATE도 있고 그렇게 해도 되고 그건 나중에..

            if (eTime <= ShrinkDuration)
            {
                eTime += Time.deltaTime;
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

        bool[] thetas = new bool[massiveSpawnDegreeDivideCount];
        float divTheta = ((Mathf.PI * 2f ) -Mathf.Deg2Rad*massiveSafetyZoneDegree )/ thetas.Length;
        int thetasNum = 0;
        for (int i = 0; i < count; i++)
        {
            var g = Instantiate(blockObject);
            g.transform.parent = transform.parent;
            g.transform.localPosition = Vector2.zero;

            int thetaNum = Random.Range(1, thetas.Length);
            float theta;
            do
            {
                if (thetas[thetasNum])
                {
                    thetasNum -= 1;
                }
                else
                {
                    thetaNum -=1;
                    if(thetaNum>0)
                    thetasNum -= 1;
                }

                if (thetasNum < 0)
                {
                    thetasNum = thetas.Length-1;
                }
            } while (thetaNum > 0);

            theta =p.getTheta() - divTheta * (thetasNum)-divTheta;

            float Range = Random.Range(RangeMin, RangeMax);
            g.GetComponent<blockBase>().setDest(theta,Range,timeOfSpawn,p);
        }
    }
    public void SpawnBlock()
    {
        float RangeMin = p.rangeMin;
        float RangeMax = p.rangeMax;

            var g = Instantiate(blockObject);
            g.transform.parent = transform.parent;
            g.transform.localPosition = Vector2.zero;
            float theta = Random.Range(0, passiveDangerZoneDegree * Mathf.Deg2Rad);
        theta = p.getTheta()+Mathf.PI +theta- passiveDangerZoneDegree * Mathf.Deg2Rad/2;
            float Range = Random.Range(RangeMin, RangeMax);
            g.GetComponent<blockBase>().setDest(theta, Range, TimeOfSpawn, p);
        g.GetComponentInChildren<Animator>().SetFloat("Speed", 1 / TimeOfSpawn);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            p.centerCollisionEnter(this);
        }
    }

}
