using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Android;

public class playerMovwe : MonoBehaviour
{
    /// <summary>
    /// ���� �����ϴ� ��
    /// </summary>

    [Header("References")]
    [SerializeField] PlayerEffects effector;
    new Rigidbody2D rigidbody;

    [Header("Values")]
    public float rangeMax =4;
    public float rangeMin =2;
    public float turnSpeed = 1;
    public float rangeSpeed = 1;

    bool isSmallPower;
    Vector2 normalCircleScale;
    Vector2 bigCircleRatio;
    public Vector2 GetBigCircleRatio()
    {
        return bigCircleRatio;
    }

    int fowardDirection;
    public int FowardDir
    {
        get
        {
            return fowardDirection;
        }
        set
        {
            fowardDirection = value;
        }
    }

    float currentRange;
    public float Range
    {
        get
        {
            return currentRange;
        }
        set
        {
            currentRange = math.clamp(value,rangeMin, rangeMax);
        }
    }
    float timeStack;
    circleStates currentState;
    bool changeState;
    // Start is called before the first frame update
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    void OnEnable()
    {
        initValues();
        StartCoroutine(FSMmain());
    }
    IEnumerator FSMmain()
    {
        while (true)
        {
            changeState = false;
            yield return StartCoroutine(Enum.GetName(typeof(circleStates), currentState));
        }
    }
    IEnumerator move()
    {
        float StartTime = 0;
        isSmallPower = true;
        float speedScale = 1;
        do
        {
            if (isSmallPower)
            {
                StartTime += Time.unscaledDeltaTime;
                speedScale = StartTime/0.2f;
                if (StartTime>=0.2f)//�뽬��, ����� �� �� �ϰ� move ���� ��� �����ð�
                {
                    isSmallPower = false;
                    speedScale = 1;
                }
            }
            rotationCircle(turnSpeed*speedScale);
            inputMoveTask();
            yield return null;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * GetCircleRotation());////////ȣ�� ��ġ ������ �ʿ��� �� ����
        } while (!changeState);
    }
    IEnumerator dash()
    {
        effector.StartDash();
        float startTime = 0;
        float dashSpeed;
        do
        {
            dashSpeed = effector.GetDashSpeed();
            startTime += Time.unscaledDeltaTime;
            rotationCircle(dashSpeed);
           
            if (startTime >= effector.FullDashDuration)
            {
                changeState = true;
                currentState = circleStates.move;
            }
            yield return null;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * GetCircleRotation());////////ȣ�� ��ġ ������ �ʿ��� �� ����
        } while (!changeState);
        isSmallPower = true;
    }
    void rotationCircle(float speed)
    {
        timeStack += Time.deltaTime * speed * fowardDirection;
        Vector2 newMove = new Vector2(bigCircleRatio.x * Range * math.cos(-timeStack), bigCircleRatio.y * Range * math.sin(-timeStack));

        newMove = Quaternion.Euler(0,0,transform.parent.rotation.eulerAngles.z)*newMove;
        newMove += (Vector2)transform.parent.position;
        rigidbody.MovePosition(newMove);
        if (timeStack >= math.PI * 2)
        {
            timeStack -= math.PI * 2;
        }
    }
    IEnumerator die()
    {
        do
        {
           
            yield return null;
        } while (!changeState);
    }
    IEnumerator fever()
    {
        do
        {

            yield return null;
        } while (!changeState);
        isSmallPower = true;
    }

    [Header("BoomValues")]
    public float chargeDuration = 0.2f;
    public float boomToCenterDuration = 0.1f;
    public float returnDuration = 0.2f;
    float waitForSpawnDuration;
    blockSpawner spawner;
    bool isSpawned = false;

    IEnumerator boom()
    {
        float eTime = 0;
        float eeTime = 0;
        isSpawned = false;
        //����::startBoom();//����°Ž�����
        effector.StartBoom();
        do
        {
            if (!isSpawned)
            {
                eTime += Time.unscaledDeltaTime;
                if (eTime <= chargeDuration)
                {//���� ����
                    float rati = eTime / chargeDuration;
                    rati = 1 + rati*0.1f;

                    Vector2 newMove = new Vector2(bigCircleRatio.x * Range * rati * math.cos(-timeStack), bigCircleRatio.y * Range * rati * math.sin(-timeStack));

                    newMove = Quaternion.Euler(0, 0, transform.parent.rotation.eulerAngles.z) * newMove;
                    newMove += (Vector2)transform.parent.position;
                    rigidbody.MovePosition(newMove);

                }
                else if (eTime <= boomToCenterDuration + chargeDuration)
                {//��Ͽ� ��
                    float rati = (eTime-chargeDuration) / (boomToCenterDuration);
                    rati = 1 - rati;

                    Vector2 newMove = new Vector2(bigCircleRatio.x * Range * rati * math.cos(-timeStack), bigCircleRatio.y * Range * rati * math.sin(-timeStack));

                    newMove = Quaternion.Euler(0, 0, transform.parent.rotation.eulerAngles.z) * newMove;
                    newMove += (Vector2)transform.parent.position;
                    rigidbody.MovePosition(newMove);
                }
            }
            if (isSpawned)
            {
                
                float rati = (eTime - chargeDuration) / (boomToCenterDuration);
                eeTime += Time.unscaledDeltaTime;
                if (eeTime <= waitForSpawnDuration)
                {//����� ���

                }
                else if (eeTime <= waitForSpawnDuration + returnDuration)
                {//���ڸ���

                    rati = 1-rati +  rati * (eeTime-waitForSpawnDuration) / returnDuration;
                    Vector2 newMove = new Vector2(bigCircleRatio.x * Range * rati * math.cos(-timeStack), bigCircleRatio.y * Range * rati * math.sin(-timeStack));

                    newMove = Quaternion.Euler(0, 0, transform.parent.rotation.eulerAngles.z) * newMove;
                    newMove += (Vector2)transform.parent.position;
                    rigidbody.MovePosition(newMove);
                }
                else
                {
                    changeState = true;
                    currentState = circleStates.move;
                }
            }
            
            yield return null;
        } while (!changeState);
        isSmallPower = true;
    }
    public void setBoom(float timeOfSpawn, blockSpawner spawner)
    {
        currentState = circleStates.boom;
        changeState = true;
        waitForSpawnDuration = timeOfSpawn;
        this.spawner = spawner;

    }
    IEnumerator idle()
    {
        do
        {

            yield return null;
        } while (!changeState);
    }
    void Update()
    {

        Vector2 currentBigCircleScale = transform.parent.localScale;
        bigCircleRatio = currentBigCircleScale / normalCircleScale;
        if (Input.GetKeyDown(KeyCode.R))//debug�� ����
        {
            initValues();
        }
    }
    public float GetCircleRotation()
    {
        return -(timeStack+math.PI/2) + transform.parent.rotation.eulerAngles.z*Mathf.Deg2Rad;
    }
    void initValues()
    {
        timeStack = -math.PI / 2;
        Range = 3;
        currentState = circleStates.move;
        changeState = true;
        fowardDirection = 1;
        normalCircleScale = transform.parent.localScale;
        bigCircleRatio = Vector2.one;
        isSmallPower = true;
    }

    void inputMoveTask()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Range += Time.deltaTime* rangeSpeed;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            Range -= Time.deltaTime* rangeSpeed;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            changeState = true;
            currentState = circleStates.dash;
        }
        
    }
    enum circleStates
    {
        idle = 0,
        move = 1,
        dash = 2,
        die = 3,
        fever = 4,
        boom = 5,//(����»���)
    }
    public bool isDashOrFever()
    {
        return currentState == circleStates.dash || currentState == circleStates.fever || isSmallPower;
    }
    public void blockCollisionEnter(blockBase block)
    {
        if (currentState != circleStates.die && currentState != circleStates.boom)
        {
            currentState = circleStates.die;
            changeState = true;
        }
    }
    public void centerCollisionEnter(blockSpawner center)
    {
        if (currentState == circleStates.boom)
        {
            spawner.cutCenter();
            isSpawned = true;
            spawner.SpawnBlocks(spawner.SpawnCount, waitForSpawnDuration);
        }
        else
        {

            if (currentState != circleStates.die)
            {
                currentState = circleStates.die;
                changeState = true;
            }
        }
    }
}
