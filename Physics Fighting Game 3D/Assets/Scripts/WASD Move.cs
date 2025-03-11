using UnityEngine;

public class WASDMovement : MonoBehaviour
{
    public float moveForce = 10f;
    public float maxVelocity = 5f;
    public float jumpForce = 5f;    // Force of the jump
    public float coyoteTime = 0.2f;  // How long after falling the player can still jump
    public ThirdPersonCamera cameraController; // Make this public so we can assign it in inspector
    
    private Rigidbody rb;
    private bool isGrounded;        // Track if we can jump
    private float coyoteTimeCounter;  // Tracks remaining coyote time
    private bool jumpRequested;     // Track if we need to jump next physics update

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Remove the automatic camera finding - we'll assign it in Inspector instead
    }

    void Update()
    {
        // Handle jump input in Update for better responsiveness
        if (Input.GetKeyDown(KeyCode.Space) && coyoteTimeCounter > 0)
        {
            jumpRequested = true;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        // Only check for ground once per frame
        if (!isGrounded)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                if (Vector3.Dot(contact.normal, Vector3.up) > 0.7f)
                {
                    isGrounded = true;
                    coyoteTimeCounter = coyoteTime;
                    return;
                }
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    void FixedUpdate()
    {
        // Add null check for camera controller
        if (cameraController == null)
        {
            Debug.LogError("No camera controller assigned to player!");
            return;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Get camera-relative directions
        Vector3 forward = cameraController.GetForwardDirection();
        Vector3 right = cameraController.GetRightDirection();

        // Calculate movement direction relative to camera
        Vector3 movement = (forward * verticalInput + right * horizontalInput);
        if (movement.magnitude > 0)
        {
            movement.Normalize();
            Vector3 force = movement * moveForce;
            rb.AddForce(force);
        }

        // Update coyote time
        if (!isGrounded)
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
        }

        // Handle jumping
        if (jumpRequested)
        {
            // Reset vertical velocity before applying jump force
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            coyoteTimeCounter = 0;
            jumpRequested = false;
        }

        // Cap the velocity
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (horizontalVelocity.magnitude > maxVelocity)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxVelocity;
            rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);
        }
    }
}
