using UnityEngine;
using UnityEngine.UI;

public class FPPWASDMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float staminaRegenRate = 10f;
    public float sprintStaminaDrainRate = 20f;
    public float jumpStaminaCost = 10f; // New variable for jump stamina cost
    public Slider staminaBar;

    [Header("Camera Settings")]
    public float mouseSensitivity = 2f;
    public Transform playerCamera;
    public float maxLookAngle = 80f;

    private CharacterController controller;
    private float verticalRotation = 0f;
    private Vector3 velocity;
    private bool isGrounded;
    public float stamina; // Changed to public
    private bool isSprinting;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        stamina = maxStamina;

        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (staminaBar != null)
        {
            staminaBar.maxValue = maxStamina;
            staminaBar.value = stamina;
        }
    }

    void Update()
    {
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

        // Handle WASD movement and sprinting
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        float currentSpeed = moveSpeed;
        isSprinting = Input.GetKey(KeyCode.LeftShift) && stamina > 0;

        if (isSprinting)
        {
            currentSpeed = sprintSpeed;
            stamina -= sprintStaminaDrainRate * Time.deltaTime;
            if (stamina < 0) stamina = 0;
        }
        else
        {
            stamina += staminaRegenRate * Time.deltaTime;
            if (stamina > maxStamina) stamina = maxStamina;
        }

        if (staminaBar != null)
        {
            staminaBar.value = stamina;
        }

        move = move.normalized; // Normalize to prevent faster diagonal movement
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded && stamina >= jumpStaminaCost)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            stamina -= jumpStaminaCost; // Reduce stamina on jump
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public bool IsSprinting // New property to check if the player is sprinting
    {
        get { return isSprinting; }
    }

    public float Speed // Property to get the player's current speed
    {
        get { return controller.velocity.magnitude; }
    }
}
