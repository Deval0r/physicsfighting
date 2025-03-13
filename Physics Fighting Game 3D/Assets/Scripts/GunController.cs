using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 20f;
    public int damage = 10;
    public Camera playerCamera;
    public Transform gunTransform;
    public float swayAmount = 0.1f;
    public float swaySpeed = 2f;
    public float breathingAmount = 0.05f;
    public float breathingSpeed = 1f;
    public float aimSwayAmount = 0.02f;
    public float aimFOV = 30f;
    public Vector3 aimPosition = new Vector3(0f, -0.1f, 0.5f);
    public float aimSpeed = 10f;
    public float aimAssistRange = 1f; // Variable for aim assist range
    public float recoilAmount = 0.1f; // Variable for recoil amount
    public float recoilSpeed = 5f; // Variable for recoil recovery speed
    public float cooldownTime = 0.5f; // Variable for cooldown time between shots
    public AudioClip shootSound; // Variable for the shooting sound

    public FPPWASDMovement playerMovement; // Reference to the FPPWASDMovement script

    private Vector3 initialGunPosition;
    private Quaternion initialGunRotation;
    private float swayTimer;
    private float breathingTimer;
    private Vector3 initialCameraPosition;
    private float initialCameraFOV;
    private float lastShotTime; // Variable to track the last shot time
    private Vector3 recoilOffset; // Variable to track recoil offset
    private AudioSource audioSource; // Variable for the AudioSource

    void Start()
    {
        initialGunPosition = gunTransform.localPosition;
        initialGunRotation = gunTransform.localRotation;
        initialCameraPosition = gunTransform.localPosition;
        initialCameraFOV = playerCamera.fieldOfView;
        playerMovement = GetComponent<FPPWASDMovement>(); // Assign FPPWASDMovement reference
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component

        // Check if AudioSource component is present
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing. Please add an AudioSource component to the gun.");
        }
    }

    void Update()
    {
        AimGun();

        if (Input.GetButtonDown("Fire1") && Time.time >= lastShotTime + cooldownTime)
        {
            Shoot();
        }

        if (Input.GetButton("Fire2"))
        {
            Aim();
        }
        else
        {
            ResetAim();
        }

        ApplySway();
        ApplyBreathing();
        ApplyRecoil();
    }

    void AimGun()
    {
        // Raycast from the center of the screen to get the point where the camera is looking
        Ray cameraRay = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        Vector3 targetPoint;

        if (Physics.Raycast(cameraRay, out hit))
        {
            targetPoint = hit.point; // If raycast hits something, aim at that point
        }
        else
        {
            targetPoint = cameraRay.GetPoint(1000); // Otherwise, aim far away in the direction the camera is looking
        }

        Vector3 direction = targetPoint - firePoint.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate the gun towards the target point
        gunTransform.rotation = Quaternion.Slerp(gunTransform.rotation, targetRotation, Time.deltaTime * 10f);
        firePoint.rotation = gunTransform.rotation;
    }

    void ApplySway()
    {
        float currentSwayAmount = swayAmount;
        float currentSwaySpeed = swaySpeed;

        if (Input.GetButton("Fire2"))
        {
            currentSwayAmount = aimSwayAmount;
        }
        else if (Input.GetKey(KeyCode.LeftShift) && playerMovement.stamina > 0)
        {
            currentSwayAmount = swayAmount * 2f; // Increase sway amount while sprinting
            currentSwaySpeed = swaySpeed * 2f; // Increase sway speed while sprinting
        }

        float moveX = Input.GetAxis("Horizontal") * currentSwayAmount;
        float moveY = Input.GetAxis("Vertical") * currentSwayAmount;

        swayTimer += Time.deltaTime * currentSwaySpeed;
        float swayX = Mathf.Sin(swayTimer) * moveX;
        float swayY = Mathf.Sin(swayTimer) * moveY;

        gunTransform.localPosition = initialGunPosition + recoilOffset + new Vector3(swayX, swayY, 0f);
        gunTransform.localRotation *= Quaternion.Euler(swayY, swayX, 0f); // Apply sway rotation on top of the current rotation
    }

    void ApplyBreathing()
    {
        breathingTimer += Time.deltaTime * breathingSpeed;
        float breathingOffset = Mathf.Sin(breathingTimer) * breathingAmount;

        if (Input.GetKey(KeyCode.LeftShift) && playerMovement.stamina > 0)
        {
            breathingOffset *= 2f; // Increase breathing effect while sprinting
        }

        gunTransform.localPosition += new Vector3(0f, breathingOffset, 0f);
    }

    void ApplyRecoil()
    {
        // Smoothly transition the gun position back to its initial position after recoil
        recoilOffset = Vector3.Lerp(recoilOffset, Vector3.zero, Time.deltaTime * recoilSpeed);
        gunTransform.localPosition += recoilOffset;
    }

    void Aim()
    {
        // Adjust the gun's local position to simulate aiming
        gunTransform.localPosition = Vector3.Lerp(gunTransform.localPosition, aimPosition, Time.deltaTime * aimSpeed);

        // Adjust the camera's field of view to simulate zooming
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, aimFOV, Time.deltaTime * aimSpeed);
    }

    void ResetAim()
    {
        // Reset the gun's local position to its initial state
        gunTransform.localPosition = Vector3.Lerp(gunTransform.localPosition, initialGunPosition, Time.deltaTime * aimSpeed);

        // Reset the camera's field of view to its initial state
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, initialCameraFOV, Time.deltaTime * aimSpeed);
    }

    void Shoot()
    {
        lastShotTime = Time.time; // Update the last shot time

        // Apply recoil offset
        recoilOffset += new Vector3(0f, 0f, -recoilAmount);

        Vector3 adjustedDirection = ApplyAimAssist(firePoint.position, firePoint.forward);

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(adjustedDirection));
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.linearVelocity = adjustedDirection * projectileSpeed;

        Projectile projScript = projectile.GetComponent<Projectile>();
        if (projScript != null)
        {
            projScript.SetDamage(damage);
        }

        // Play shooting sound with slight pitch variation
        if (audioSource != null && shootSound != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f); // Random pitch variation between 0.9 and 1.1
            audioSource.PlayOneShot(shootSound);
        }
    }

    Vector3 ApplyAimAssist(Vector3 firePoint, Vector3 fireDirection)
    {
        RaycastHit hit;
        if (Physics.SphereCast(firePoint, aimAssistRange, fireDirection, out hit))
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                Vector3 aimAssistDirection = (hit.point - firePoint).normalized;
                return Vector3.Lerp(fireDirection, aimAssistDirection, 0.5f); // Adjust the blend factor as needed
            }
        }
        return fireDirection;
    }

}
