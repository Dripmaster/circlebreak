using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class blockSpawner : MonoBehaviour
{
    public Color MainColor
    {
        get { return spriteRenderer.color; }
    }
    // Start is called before the first frame update
    public GameObject blockObject;
    public GameObject wallObject;
    public GameObject smallCircle;
    public playerMovwe p;
    SpriteRenderer spriteRenderer;

    public float fillSpeed = 1;

    float scaleFactor;
    float toScaleFactor;
    Vector2 normalScale;
    float eTime =  10000;

    bool newState;
    bool boomStart;

    [Header("Colors")]
    [SerializeField] Color wallColor;
    Color blockColor;

    [Header("MassiveSpawnValues")]
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
    public float RangeOfLevel1 =2.5f;
    public int CountOfSpawnLevel1 = 5;
    public float RangeOfLevel2 =4.5f;
    public int CountOfSpawnLevel2 = 7;
    public int CountOfSpawnLevel3 = 9;
    int wallspawnCounts = 0;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        scaleFactor = 1;
        normalScale = transform.localScale;
        wallspawnCounts = 0;
        rangeSetCount = 0;
        rangeCount = 0;
        usedRange = new bool[RangeDivCount];
        newState = false;
        boomStart = false;
    }
    public void Activate()
    {
        StartCoroutine(spawn());
        StartCoroutine(idle());
    }
    public void DeActivate()
    {
        StopAllCoroutines();
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
                if(!boomStart)
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
            if (boomStart||eTime >= duration)
            {
                if (dir==1)
                {
                    tempScaleFactor = scaleFactor;
                }
                scaleFactor = x;
                break;
            }
            transform.localScale = normalScale * x;
            yield return null;
        } while (!boomStart);

    }
    IEnumerator idle()
    {
        do
        {
            if (!newState && !p.isFever())
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
                        boomStart = false;
                    }
                }
                else if (!p.isBoom())
                {
                    scaleFactor += Time.deltaTime * fillSpeed;
                    transform.localScale = normalScale * scaleFactor;
                }
                if (VectorUtils.CompareSize(transform.localScale, smallCircle.transform.localScale) > 0)
                {
                    p.centerCollisionEnter(this);
                }
            }
            yield return null;
        } while (true);
    }
    // Update is called once per frame
    void Update()
    {            
        if (!p.isClear()&&!p.isReady()&&!p.isDashOrFever()&&!p.isDie()&&!p.isBoom()&&Input.GetKeyDown(KeyCode.Q))
        {
            p.setBoom(this);
            boomStart = true;
        }
        blockColor = spriteRenderer.color;
    }
    public void cutCenter(float scaleF = 0.5f)
    {
        toScaleFactor = scaleF;
        eTime = 0;
    }
   public int getCountofSpawn()
    {
        float s = transform.localScale.x;
        int returnCount = 0;

        if(s <= RangeOfLevel1)
        {
            returnCount = CountOfSpawnLevel1;
        }
        else if (s <= RangeOfLevel2)
        {

            returnCount = CountOfSpawnLevel2;
        }
        else
        {
            returnCount = CountOfSpawnLevel3;
        }


        return returnCount;
    }
    public enum blockType
    {
        block =0,
        wall = 1
    }
    public GameObject generateObject(blockType t)
    {
        GameObject obj = t == 0 ? blockObject : wallObject;
        var g = Instantiate(obj);
        g.transform.parent = transform.parent;
        g.transform.localPosition = Vector2.zero;
        if(t == blockType.block)
        {
            g.GetComponent<blockBase>().spriteRenderer.color = blockColor;
        }
        else if(t== blockType.wall)
        {
            g.GetComponent<blockBase>().spriteRenderer.color = wallColor;
        }
        return g;
    }
    public GameObject generateObject(GameObject obj)
    {
        var g = Instantiate(obj);
        g.transform.parent = transform.parent;
        g.transform.localPosition = Vector2.zero;
        return g;
    }
    public void SpawnBlocks()
    {
        float RangeMin = p.rangeMin;
        float RangeMax = p.rangeMax;
        int count = getCountofSpawn();

        bool[] thetas = new bool[massiveSpawnDegreeDivideCount];
        float divTheta = ((p.FowardDir*(Mathf.PI * 2f ) -Mathf.Deg2Rad*massiveSafetyZoneDegree) )/ thetas.Length;
        int thetasNum = 0;
        for (int i = 0; i < count; i++)
        {
            GameObject g = generateObject(blockType.block);

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
            g.GetComponent<blockBase>().setDest(theta,Range, TimeOfSpawn, p);
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
            g = generateObject(blockType.wall);
            RangeIdx = getRandomRangeWallidx(UnityEngine.Random.Range(0, RangeDivCount));
        }
        else
        {
            RangeIdx = UnityEngine.Random.Range(0, RangeDivCount);
            g = generateObject(blockType.block);
        }
            float theta = UnityEngine.Random.Range(0, passiveDangerZoneDegree * Mathf.Deg2Rad);
        theta = p.getTheta()+Mathf.PI +theta- passiveDangerZoneDegree * Mathf.Deg2Rad/2;
            g.GetComponent<blockBase>().setDest(theta, RangeMin + RangeLevel * RangeIdx, TimeOfSpawn, p);
    }
    public GameObject SpawnButton(GameObject obj)
    {
        float RangeMin = p.rangeMin;
        float RangeMax = p.rangeMax;
        float RangeMinToMax = RangeMax - RangeMin;
        float RangeLevel = RangeMinToMax / RangeDivCount;

        GameObject g = generateObject(obj); ;
        int RangeIdx = 0;
        RangeIdx = UnityEngine.Random.Range(0, RangeDivCount);
        float theta = UnityEngine.Random.Range(0, passiveDangerZoneDegree * Mathf.Deg2Rad);
        theta = p.getTheta() + Mathf.PI + theta - passiveDangerZoneDegree * Mathf.Deg2Rad / 2;
        g.GetComponent<blockBase>().setDest(theta, RangeMin + RangeLevel * RangeIdx, TimeOfSpawn, p);
        return g;
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
        if(p.isBoom()&&collision.tag == "Player")
        {
            p.centerCollisionEnter(this);
        }   
    }
}
