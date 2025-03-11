using UnityEngine;

public class FPPWASDMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;
    
    [Header("Camera Settings")]
    public float mouseSensitivity = 2f;
    public Transform playerCamera;
    public float maxLookAngle = 80f;

    private CharacterController controller;
    private float verticalRotation = 0f;
    private Vector3 velocity;
    private bool isGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Add this debug line
        Debug.Log($"Horizontal: {Input.GetAxis("Horizontal")}, Vertical: {Input.GetAxis("Vertical")}");
        
        // Check if player is grounded
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Handle mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the camera up/down
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
        playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // Rotate the player left/right
        transform.Rotate(Vector3.up * mouseX);

        // Handle WASD movement
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        // Create movement vector relative to camera's forward direction
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        move = move.normalized; // Normalize to prevent faster diagonal movement
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
