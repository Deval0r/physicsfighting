using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;            // The player cube
    public float distance = 10.0f;      // Distance from player
    public float height = 5.0f;         // Height above player
    public float mouseSensitivity = 2.0f;
    public float minVerticalAngle = -30.0f;
    public float maxVerticalAngle = 60.0f;
    
    private float rotationX = 0;
    private float rotationY = 0;
    private Vector3 currentRotation;
    private Vector3 smoothVelocity = Vector3.zero;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Initialize camera position
        rotationY = transform.eulerAngles.y;
    }

    void LateUpdate()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Calculate rotation
        rotationY += mouseX;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

        // Calculate camera position
        Vector3 targetRotation = new Vector3(rotationX, rotationY, 0);
        transform.eulerAngles = targetRotation;

        // Position the camera
        Vector3 negDistance = new Vector3(0.0f, height, -distance);
        Vector3 position = target.position + transform.rotation * negDistance;

        transform.position = position;
    }

    public Vector3 GetForwardDirection()
    {
        // Get the camera's forward direction, but flatten it to the XZ plane
        Vector3 forward = transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    public Vector3 GetRightDirection()
    {
        // Get the camera's right direction, but flatten it to the XZ plane
        Vector3 right = transform.right;
        right.y = 0;
        return right.normalized;
    }
} 