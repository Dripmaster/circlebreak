using UnityEngine;

public class CameraEffector : MonoBehaviour
{
    public float CurrentZoom { get { return cameraZoom; } }

    [SerializeField] float followSpeed;
    float originalSize;
    Vector3 originalPos;
    Vector3 targetPos;
    float cameraZoom = 0;
    Camera cameraComponent;


    private void Awake()
    {
        cameraComponent = GetComponent<Camera>();
        originalSize = cameraComponent.orthographicSize;
        originalPos = transform.position;
    }
    public void SetZoom(float magnitude)
    {
        cameraComponent.orthographicSize = originalSize - magnitude;
        cameraZoom = magnitude;
    }
    public void SetFollow(Vector3 targetPosition)
    {
        targetPos = targetPosition;
    }
    private void Update()
    {
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
        }
    }
}
