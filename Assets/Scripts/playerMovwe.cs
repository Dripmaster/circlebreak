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
    public  PlayerEffects effector;
    new Rigidbody2D rigidbody;
    public CapsuleCollider2D cap;


    [Header("Values")]
    public float rangeMax =4;
    public float rangeMin =2;
    public float turnSpeed = 1;
    public float rangeSpeed = 1;
    public float invincibleTime = 0.4f;
    public float moveSpeedReloadTime = 0.4f;

    [Header("DashValues")]
    public float dashCoolTime = 0.5f;
    float dashCoolTimeTimer = 0f;


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
        cap.enabled = false;
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
        float eTime = 0;
        isSmallPower = true;
        float speedScale = 0;
        effector.TurnWalkParticle(true);
        do
        {
            if (dashCoolTimeTimer >= 0)
            {
                dashCoolTimeTimer -= Time.unscaledDeltaTime;
            }
            if (isSmallPower)
            {
                StartTime += Time.unscaledDeltaTime;
                if (StartTime>= invincibleTime)//�뽬��, ����� �� �� �ϰ� move ���� ��� �����ð�
                {
                    isSmallPower = false;
                    speedScale = 1;
                }
            }
            if (eTime <= moveSpeedReloadTime)
            {
                speedScale = Mathf.Lerp(0, 1, eTime / moveSpeedReloadTime);
                eTime += Time.unscaledDeltaTime;
                if (eTime >= speedScale)
                {
                    speedScale = 1;
                }
            }
            rotationCircle(turnSpeed*speedScale);
            inputMoveTask();
            yield return null;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * GetCircleRotation());////////ȣ�� ��ġ ������ �ʿ��� �� ����
        } while (!changeState);
        effector.TurnWalkParticle(false);
    }
    public void setDashCollider(bool value)
    {
        cap.enabled = value;
    }
    IEnumerator dash()
    {
        effector.StartDash();
        float startTime = 0;
        float dashSpeed;

        isSmallPower = true;
        dashCoolTimeTimer = dashCoolTime;
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
        cap.enabled = false;
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
            effector.OnDead();
        do
        {
            yield return null;
        } while (!changeState);
    }

    [Header("FeverValues")]
    public float feverDuration = 10f;
    public float feverSpeed = 5;
    public void setFever()
    {
        changeState = true;
        currentState = circleStates.fever;
    }
    public bool isFever() { return currentState == circleStates.fever; }
    IEnumerator fever()
    {
        float startTime = 0;
        do
        {
            startTime += Time.unscaledDeltaTime;
            if (startTime >= feverDuration)
            {
                changeState = true;
                currentState = circleStates.move;
            }
            rotationCircle(feverSpeed);
            inputMoveTask();
            yield return null;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * GetCircleRotation());////////ȣ�� ��ġ ������ �ʿ��� �� ����
        } while (!changeState);
        isSmallPower = true;
    }

    public void ReadyDone()
    {
        changeState = true;
        currentState = circleStates.move;
    }
    IEnumerator ready()
    {
        do
        {
            yield return null;
        } while (!changeState);
    }

    [Header("BoomValues")]
    public float chargeDuration = 0.2f;
    public float chargeDistance = 0.3f;
    public float boomToCenterDuration = 0.1f;
    public float returnDuration = 0.2f;
    public float waitForSpawnDuration = 0.5f;
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
                    float x = eTime / chargeDuration;
                    float distance = Mathf.Lerp(0, chargeDistance, TimeCurves.ExponentialMirrored(x));
                    Vector2 newMove = new Vector2(bigCircleRatio.x * (Range + distance) * math.cos(-timeStack), bigCircleRatio.y * (Range + distance) * math.sin(-timeStack));

                    newMove = Quaternion.Euler(0, 0, transform.parent.rotation.eulerAngles.z) * newMove;
                    newMove += (Vector2)transform.parent.position;
                    rigidbody.MovePosition(newMove);

                }
                else if (eTime <= boomToCenterDuration + chargeDuration)
                {//��Ͽ� ��
                    float rati = (eTime-chargeDuration) / (boomToCenterDuration);
                    

                    float x;
                    x = Mathf.Lerp(0, 1f, rati);

                    rati = 1 - x;

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

                    float x;
                    float r = (eeTime - waitForSpawnDuration) / returnDuration;
                    x = Mathf.Lerp(0,1,TimeCurves.ExponentialMirrored(r));
                    rati = 1 - rati + rati * x;


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
    public void setBoom(blockSpawner spawner)
    {
        currentState = circleStates.boom;
        changeState = true;
        this.spawner = spawner;

    }
    public bool isBoom()
    {
        return currentState == circleStates.boom;

    }
    public bool isDie()
    {
        return currentState == circleStates.die;

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
        currentState = circleStates.ready;
        changeState = true;
        fowardDirection = 1;
        normalCircleScale = transform.parent.localScale;
        bigCircleRatio = Vector2.one;
        isSmallPower = true;
    }
    public float getTheta()
    {
        return timeStack;
    }
    void inputMoveTask()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            effector.OnTurn(true);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            effector.OnTurn(false);

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
            if (currentState!=circleStates.fever&&dashCoolTimeTimer <= 0)
            {
                changeState = true;
                currentState = circleStates.dash;
            }
        }
        
    }
    enum circleStates
    {
        ready = -1,
        idle = 0,
        move = 1,
        dash = 2,
        die = 3,
        fever = 4,
        boom = 5,//(����»���)
    }
    public bool isDashOrFever()
    {
        return currentState == circleStates.dash || currentState == circleStates.fever;
    }
    public void blockCollisionEnter(blockBase block)
    {
        if (currentState != circleStates.die && currentState != circleStates.boom && !isSmallPower)
        {
            currentState = circleStates.die;
            changeState = true;
        }
    }
    public void wallCollisionEnter(blockBase block)
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
            effector.OnBoom();
            spawner.cutCenter();
            isSpawned = true;
            spawner.SpawnBlocks(spawner.SpawnCount, spawner.TimeOfSpawn);
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
