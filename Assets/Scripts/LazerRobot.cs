using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerRobot : MonoBehaviour
{
     enum RobotState
    {
        REST = 0,
        ENTERING = 1, 
        SHOOTING = 2,
        EXITING = 3
    }

    public playerMovwe _playerMove;
    public GameObject lazerPrefab;
    public GameObject lazerLinePrefab;
    private GameObject lazer;
    private GameObject lazerLine;

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
        float innerCircleRadius = 1.25f;
        var (innerCircleTangent1, innerCircleTangent2) = getTangentToCircle(destination.x, destination.y, innerCircleRadius);

        float outerCircleRadius = 3f;
        var (outerCircleTangent1, outerCircleTangent2) = getTangentToCircle(destination.x, destination.y, outerCircleRadius);

        int random = Random.Range(0, 2);

        float deltaAngle;

        float deltaAngle1 = Mathf.Abs(innerCircleTangent1 - outerCircleTangent1);
        float deltaAngle2 = Mathf.Abs(innerCircleTangent2 - outerCircleTangent2);
        deltaAngle = deltaAngle1 > deltaAngle2? deltaAngle2 : deltaAngle1;
        float randomAngle = Random.Range(0, Mathf.Rad2Deg *  deltaAngle);
        float angle;

        if(random == 0) {
            angle = innerCircleTangent1 * Mathf.Rad2Deg - randomAngle;
        }

        else {
            angle = innerCircleTangent2 * Mathf.Rad2Deg + randomAngle;
        }

        targetRotation = Quaternion.Euler(0, 0, angle);
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
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 2);
            
            if(Vector2.Distance(transform.position, destination) < 0.1 && 
            Mathf.Abs(Mathf.Round(Quaternion.Dot(transform.rotation, targetRotation) * 100000)) == 100000) {
                RestEndDelegate endAction = () => {
                    lazer = Instantiate(lazerPrefab, transform.position, transform.rotation);
                    lazer.GetComponent<LazerBase>()._playerMove = _playerMove;

                    float sizeX = lazer.GetComponent<BoxCollider2D>().size.x;
                    lazer.GetComponent<LazerBase>().transform.position = getMuzzlePosition(sizeX);
                    lazer.GetComponent<LazerBase>().transform.localScale = new Vector2(getLazerScale(sizeX), 1);
                    Destroy(lazerLine.gameObject);
                };
                
                lazerLine = Instantiate(lazerLinePrefab, transform.position, transform.rotation);
                float sizeX = lazerLine.GetComponent<SpriteRenderer>().bounds.size.x;
                lazerLine.gameObject.transform.position = getMuzzlePosition(2.53f);
                lazerLine.gameObject.transform.localScale = new Vector2(getLazerScale(2.53f), 1);

                changeState(RobotState.SHOOTING, 1f, endAction);
            }
        }

        if(robotState == RobotState.SHOOTING) {
            shootingTimer += Time.deltaTime;

            if(shootingTimer > 2) {
                changeState(RobotState.EXITING, 1f);
                Destroy(lazer.gameObject);
            }
        }

        if(robotState == RobotState.EXITING) {
            transform.position = Vector2.Lerp(transform.position, source, Time.deltaTime * 2);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * 5);

            if(Vector2.Distance(transform.position, source) < 0.1 && 
            Mathf.Abs(Mathf.Round(Quaternion.Dot(transform.rotation, Quaternion.identity) * 100000)) == 100000) {
                Destroy(gameObject);
            }                
        }
    }

    public (float tangent1, float tangent2) getTangentToCircle(float positionX, float positionY, float radius) {

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

    public Vector2 getMuzzlePosition(float sizeX) {
        Vector3 position = transform.position;
        float scaleX = getLazerScale(sizeX);
        
        float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector3 centerVector = new Vector3(Mathf.Cos(angle) * ((sizeX * scaleX) / 2), Mathf.Sin(angle) * ((sizeX * scaleX) / 2), 0);
        Vector3 muzzleVector = new Vector3(Mathf.Cos(angle) * (GetComponent<SpriteRenderer>().bounds.size.x - 0.2f) / 2, 
                                           Mathf.Sin(angle) * (GetComponent<SpriteRenderer>().bounds.size.x - 0.2f) / 2, 0);

        position += centerVector;
        position += muzzleVector;

        return position;
    }

    public float getLazerScale(float sizeX) {
        float scaleX = 26 / sizeX;
        return scaleX;
    }

    public void setPositions(Vector2 source, Vector2 destination) {
        transform.position = source;
        this.source = source;
        this.destination = destination;
    }
}
