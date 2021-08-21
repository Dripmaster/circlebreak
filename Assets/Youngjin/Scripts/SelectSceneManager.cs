using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectSceneManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] string[] sceneNames;
    [SerializeField] CameraEffector cameraEffector;
    [SerializeField] Transform[] mapPoints;
    [SerializeField] Color[] transitionColors;
    [SerializeField] Transform mapTransform;
    [SerializeField] Transform playerTransform;

    [Header("Effect Settings")]
    [SerializeField] float moveMapDuration;
    [SerializeField] float moveZoom;
    [SerializeField] float sceneChangeDuration;
    [SerializeField] float sceneChangeCircleScale;
    [SerializeField] float sceneChangeZoom;


    bool isMovable = false;
    bool fixCameraToPlayer = false;
    int currentPoint;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    IEnumerator MoveMap(int dir)
    {
        if ((dir == 1 && currentPoint == mapPoints.Length - 1)
            || (dir == -1 && currentPoint == 0))
            yield break;
        cameraEffector.SetFollow(mapPoints[currentPoint + dir].position);
        StartCoroutine(ZoomCamera(moveZoom, 0.3f));
        isMovable = false;
        float eTime = 0f;
        while(eTime < moveMapDuration)
        {
            yield return null;
            eTime += Time.fixedDeltaTime;
            float x = eTime / moveMapDuration;
            playerTransform.position = new Vector3( Mathf.Lerp(mapPoints[currentPoint].position.x, 
                mapPoints[currentPoint+dir].position.x, x), 0, 0);
        }
        currentPoint += dir;
        playerTransform.transform.position = new Vector3(mapPoints[currentPoint].transform.position.x,0,0);
        isMovable = true;
        StartCoroutine(ZoomCamera(0f, 0.3f));
    }
    IEnumerator ZoomCamera(float zoomTarget, float duration)
    {
        float eTime = 0f;
        float originalSize = cameraEffector.CurrentZoom;
        while (eTime < duration)
        {
            yield return null;
            float x = eTime / duration;
            {
                cameraEffector.SetZoom(Mathf.Lerp(originalSize, zoomTarget, 1 - Mathf.Pow((x - 1), 2)));
            }
            eTime += Time.unscaledDeltaTime;
        }
        cameraEffector.SetZoom(zoomTarget);
    }
    public void ZoomOut(float duration)
    {
        StartCoroutine(ZoomCamera(0, duration));
    }
    IEnumerator SelectMap()
    {
        animator.SetTrigger("Select");
        StartCoroutine(ZoomCamera(sceneChangeZoom, 0.5f));
        fixCameraToPlayer = true;
        
        yield return null;
    }
    public void OnSelectAnimDone()
    {
        StartCoroutine(SceneTransition());
    }
    IEnumerator SceneTransition()
    {
        float eTime = 0f;
        Vector3 originalScale = mapPoints[currentPoint].localScale;
        SpriteRenderer spriteRenderer = mapPoints[currentPoint].GetComponent<SpriteRenderer>();
        Color originalColor = spriteRenderer.color;
        spriteRenderer.sortingOrder = 100;
        cameraEffector.Shake(0.4f, 0.35f);
        
        while(eTime < sceneChangeDuration)
        {
            yield return null;
            eTime += Time.unscaledDeltaTime;
            float x = TimeCurves.ExponentialMirrored(eTime / sceneChangeDuration);
            mapPoints[currentPoint].localScale = originalScale + 
                Mathf.Lerp(0,sceneChangeCircleScale-1,x)*new Vector3(1, 1, 0);
            spriteRenderer.color = Color.Lerp(originalColor, transitionColors[currentPoint], x);
        }
        mapPoints[currentPoint].localScale = new Vector3(sceneChangeCircleScale, sceneChangeCircleScale, 1);
        spriteRenderer.color = transitionColors[currentPoint];
        //Change Scene
        if(sceneNames[currentPoint] != "")
            SceneManager.LoadScene(sceneNames[currentPoint]);
    }
    public void OnEnterAnimDone()
    {
        isMovable = true;
    }
    void Update()
    {
        if(isMovable)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                StartCoroutine(MoveMap(-1));
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                StartCoroutine(MoveMap(1));
            if (Input.GetKeyDown(KeyCode.Space))
                StartCoroutine(SelectMap());
        }
        if (fixCameraToPlayer)
            cameraEffector.SetFollow(playerTransform.GetChild(0).position);
    }
}
