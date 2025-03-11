using UnityEngine;
using System.Collections;

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

    [Header("Dash Settings")]
    public float dashForce = 20f;
    public float dashCooldown = 1f;
    public float dashDuration = 0.2f;
    private bool canDash = true;
    private bool isDashing = false;

    [Header("Dash Effects")]
    public ParticleSystem dashEffect;
    public AudioClip dashSound;

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

        // Handle dash input
        if (Input.GetMouseButtonDown(1) && canDash && !isDashing)
        {
            StartCoroutine(PerformDash());
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

    private IEnumerator PerformDash()
    {
        // Start dash
        canDash = false;
        isDashing = true;

        // Store current velocity
        Vector3 originalVelocity = rb.linearVelocity;

        // Get movement direction (if no input, use the direction we're moving)
        Vector3 dashDirection;
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (Mathf.Approximately(horizontalInput, 0f) && Mathf.Approximately(verticalInput, 0f))
        {
            // If no input, dash in the direction we're moving horizontally
            Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            dashDirection = horizontalVelocity.normalized;
            // If we're not moving horizontally, dash forward relative to camera
            if (dashDirection.magnitude < 0.1f)
            {
                dashDirection = cameraController.GetForwardDirection();
            }
        }
        else
        {
            // Use input direction relative to camera
            dashDirection = (cameraController.GetForwardDirection() * verticalInput + 
                           cameraController.GetRightDirection() * horizontalInput).normalized;
        }

        // Apply dash force
        rb.linearVelocity = dashDirection * dashForce;

        // Play effects
        if (dashEffect != null) dashEffect.Play();
        if (dashSound != null) AudioSource.PlayClipAtPoint(dashSound, transform.position);

        // Wait for dash duration
        yield return new WaitForSeconds(dashDuration);

        // End dash
        isDashing = false;

        // Wait for cooldown
        yield return new WaitForSeconds(dashCooldown - dashDuration);
        canDash = true;
    }

    void FixedUpdate()
    {
        // Skip normal movement processing if dashing
        if (isDashing) return;

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
