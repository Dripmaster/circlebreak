using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class blockSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject blockObject;
    public GameObject wallObject;
    public playerMovwe p;

    public float fillSpeed = 1;

    float scaleFactor;
    float toScaleFactor;
    Vector2 normalScale;
    float eTime =  10000;

    bool newState;

    [Header("MassiveSpawnValues")]
    public int SpawnCount = 10;
    public float TimeOfSpawn = 0.5f;
    public float ShrinkDuration = 0.5f;
    public float massiveSafetyZoneDegree = 90f;
    public int massiveSpawnDegreeDivideCount = 27;
    [Header("PassiveSpawnValues")]
    public float passiveSpawnTime = 1;
    public float passiveDangerZoneDegree= 45f;
    public int WallSpawnCount = 5;
    public float spawnLageScale = 1.1f;
    public float spawnLageScaleDuration = 0.3f;
    [Header("GeneralValues")]
    public int RangeDivCount = 4;
    int wallspawnCounts = 0;

    void Awake()
    {
        
    }
    private void OnEnable()
    {
        scaleFactor = 1;
        normalScale = transform.localScale;
        StartCoroutine(spawn());
        StartCoroutine(idle());
        wallspawnCounts = 0;
        rangeSetCount = 0;
        rangeCount = 0;
        usedRange = new bool[RangeDivCount];
        newState = false;
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
                newState = true;
                yield return StartCoroutine(spawn_scale(spawnLageScaleDuration));
                SpawnBlock();
                yield return StartCoroutine(spawn_scale(spawnLageScaleDuration,-1));
                newState = false;
            }
            yield return null;
        } while (true);
    }
    float tempScaleFactor;
    IEnumerator spawn_scale(float duration,int dir = 1)
    {
        float eTime = 0;
        do
        {
            eTime += Time.unscaledDeltaTime;
            float x;
            
            if (dir == -1)
            {
                x = Mathf.Lerp(scaleFactor, tempScaleFactor, eTime / duration);
            }
            else
            {
                x = Mathf.Lerp(scaleFactor, scaleFactor * spawnLageScale, eTime / duration);
            }
            transform.localScale = normalScale * x;
            if (eTime >= duration)
            {
                if (dir==1)
                {
                    tempScaleFactor = scaleFactor;
                }
                scaleFactor = x;
                break;
            }
            yield return null;
        } while (true);
    }
    IEnumerator idle()
    {
        do
        {
            if (!newState)
            {
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
                else if (!p.isBoom())
                {
                    scaleFactor += Time.deltaTime * fillSpeed;
                    transform.localScale = normalScale * scaleFactor;
                }
            }
            yield return null;
        } while (true);
    }
    // Update is called once per frame
    void Update()
    {            
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

            int thetaNum = UnityEngine.Random.Range(1, thetas.Length);
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

            float Range = UnityEngine.Random.Range(RangeMin, RangeMax);
            g.GetComponent<blockBase>().setDest(theta,Range,timeOfSpawn,p);
        }
    }
    public void SpawnBlock()
    {
        float RangeMin = p.rangeMin;
        float RangeMax = p.rangeMax;
        float RangeMinToMax = RangeMax - RangeMin;
        float RangeLevel = RangeMinToMax / RangeDivCount;
        
        wallspawnCounts++;
        GameObject g;

        int RangeIdx =0;
        if (wallspawnCounts >= WallSpawnCount)
        {
            wallspawnCounts = 0;
            g = Instantiate(wallObject);
            RangeIdx = getRandomRangeWallidx(UnityEngine.Random.Range(0, RangeDivCount));
        }
        else
        {
            RangeIdx = UnityEngine.Random.Range(0, RangeDivCount);
            g = Instantiate(blockObject);
        }
            g.transform.parent = transform.parent;
            g.transform.localPosition = Vector2.zero;
            float theta = UnityEngine.Random.Range(0, passiveDangerZoneDegree * Mathf.Deg2Rad);
        theta = p.getTheta()+Mathf.PI +theta- passiveDangerZoneDegree * Mathf.Deg2Rad/2;
            g.GetComponent<blockBase>().setDest(theta, RangeMin+RangeLevel*RangeIdx, TimeOfSpawn, p);
        g.GetComponentInChildren<Animator>().SetFloat("Speed", 1 / TimeOfSpawn);
    }
    bool[] usedRange;
    int rangeCount;
    int rangeSetCount;
    int getRandomRangeWallidx(int idx)
    {
        if (rangeSetCount >= RangeDivCount)
        {
            usedRange = new bool[RangeDivCount];
        }
        int id = idx;
        do
        {
            if (usedRange[rangeCount])
            {
                rangeCount -= 1;
            }
            else
            {
                id -= 1;
                if (id > 0)
                    rangeCount -= 1;
            }

            if (rangeCount < 0)
            {
                rangeCount = usedRange.Length - 1;
            }
        } while (id > 0);

        rangeSetCount++;

        return rangeCount;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            p.centerCollisionEnter(this);
        }
    }

}
