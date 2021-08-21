using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerRobot : MonoBehaviour
{
     enum RobotState
    {
        REST = 0,
        ENTERING = 1, 
        START_ROTATING = 2,
        SHOOTING = 3,
        END_ROTATING = 4,
        EXITING = 5
    }

    public playerMovwe _playerMove;
    public GameObject lazerPrefab;
    private GameObject lazer;

    delegate void RestEndDelegate();    
    private RestEndDelegate restEndListener = null;
    private RobotState robotState = RobotState.ENTERING;
    private RobotState nextState;
    private float targetRestTimer = 0f;
    private float restTimer = 0f;

    private Vector2 destination;
    private Vector2 source;
    
    private Quaternion targetRotation;

    private float shootingTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(robotState == RobotState.REST) {
            restTimer += Time.deltaTime;

            if(restTimer > targetRestTimer) {
                robotState = nextState;

                if(restEndListener != null) restEndListener();
                restEndListener = null;
            }
        }

        if(robotState == RobotState.ENTERING) {
            transform.position = Vector2.Lerp(transform.position, destination, Time.deltaTime * 2);
            if(Vector2.Distance(transform.position, destination) < 0.1) {
                RestEndDelegate endAction = () => {
                    float innerCircleRadius = 3.0f;
                    var (innerCircleTangent1, innerCircleTangent2) = getTangentToCircle(innerCircleRadius);

                    float outerCircleRadius = 4.5f;
                    var (outerCircleTangent1, outerCircleTangent2) = getTangentToCircle(outerCircleRadius);


                    int random = Random.Range(0, 2);
                    float startTangent;
                    float endTangent;
                    if(random == 0) {
                        startTangent = Mathf.Min(innerCircleTangent1, outerCircleTangent1);
                        endTangent = Mathf.Max(innerCircleTangent1, outerCircleTangent1);
                    }

                    else {
                        startTangent = Mathf.Min(innerCircleTangent2, outerCircleTangent2);
                        endTangent = Mathf.Max(innerCircleTangent2, outerCircleTangent2);
                    }

                    float angle = Mathf.Rad2Deg * Random.Range(startTangent, endTangent);
                    targetRotation = Quaternion.Euler(0, 0, angle);
                };

                changeState(RobotState.START_ROTATING, 0.5f, endAction);
            }
        }

        if(robotState == RobotState.START_ROTATING) {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5);
            if (Mathf.Abs(Mathf.Round(Quaternion.Dot(transform.rotation, targetRotation) * 100000)) == 100000) {
                RestEndDelegate endAction = () => {
                    lazer = Instantiate(lazerPrefab, transform.position, transform.rotation);
                    float sizeX = lazer.GetComponent<BoxCollider2D>().size.x;
                    float scaleX = 26 / sizeX;
                    lazer.GetComponent<LazerBase>().transform.localScale = new Vector2(scaleX, 1);

                    float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
                    Vector3 centerVector = new Vector3(Mathf.Cos(angle) * ((sizeX * scaleX) / 2), Mathf.Sin(angle) * ((sizeX * scaleX) / 2), 0);
                    lazer.GetComponent<LazerBase>().transform.position += centerVector;

                    lazer.GetComponent<LazerBase>()._playerMove = _playerMove;
                };

                changeState(RobotState.SHOOTING, 1f, endAction);
            }

        }

        if(robotState == RobotState.SHOOTING) {
            shootingTimer += Time.deltaTime;

            if(shootingTimer > 2) {
                changeState(RobotState.END_ROTATING, 1f);
                Destroy(lazer.gameObject);
            }
        }

        if(robotState == RobotState.END_ROTATING) {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * 5);

            if (Mathf.Abs(Mathf.Round(Quaternion.Dot(transform.rotation, Quaternion.identity) * 100000)) == 100000) {
                changeState(RobotState.EXITING, 1f);
            }
        }

        if(robotState == RobotState.EXITING) {
            transform.position = Vector2.Lerp(transform.position, source, Time.deltaTime * 2);

            if(Vector2.Distance(transform.position, source) < 0.1) {
                Destroy(gameObject);
            }                
        }
    }

    public (float tangent1, float tangent2) getTangentToCircle(float radius) {
        float positionX = transform.position.x;
        float positionY = transform.position.y;

        float distance = Mathf.Sqrt(positionX * positionX + positionY * positionY);
        float th = Mathf.Acos(radius / distance);
        float d = Mathf.Atan2(positionY, positionX);
       
        float d1 = d + th;
        float d2 = d - th;

        float tangentPoint1X = radius * Mathf.Cos(d1);
        float tangentPoint1Y = radius * Mathf.Sin(d1);
        float tangentPoint2X = radius * Mathf.Cos(d2);
        float tangentPoint2Y = radius * Mathf.Sin(d2);


        float tangent1 = Mathf.Atan2(tangentPoint1Y - positionY, tangentPoint1X - positionX);
        float tangent2 = Mathf.Atan2(tangentPoint2Y - positionY, tangentPoint2X - positionX);

        return (tangent1, tangent2);
    }

    private void changeState(RobotState state, float restTime = 0f, RestEndDelegate restEndListener = null) {
        if(restTime <= 0) {
            robotState = state;
            return ;
        }

        robotState = RobotState.REST;
        restTimer = 0f;
        targetRestTimer = restTime;
        nextState = state;

        this.restEndListener = restEndListener;
    }

    public void setPositions(Vector2 source, Vector2 destination) {
        transform.position = source;
        this.source = source;
        this.destination = destination;
    }
}
