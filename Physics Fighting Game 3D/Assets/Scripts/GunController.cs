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

    public FPPWASDMovement playerMovement; // Reference to the FPPWASDMovement script

    private Vector3 initialGunPosition;
    private Quaternion initialGunRotation;
    private float swayTimer;
    private float breathingTimer;
    private Vector3 initialCameraPosition;
    private float initialCameraFOV;

    void Start()
    {
        initialGunPosition = gunTransform.localPosition;
        initialGunRotation = gunTransform.localRotation;
        initialCameraPosition = gunTransform.localPosition;
        initialCameraFOV = playerCamera.fieldOfView;
        playerMovement = GetComponent<FPPWASDMovement>(); // Assign FPPWASDMovement reference
    }

    void Update()
    {
        AimGun();
        ApplySway();
        ApplyBreathing();

        if (Input.GetButtonDown("Fire1"))
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
    }

    void AimGun()
    {
        // Align the gun and firePoint with the camera's forward direction
        Vector3 cameraForward = playerCamera.transform.forward;
        gunTransform.rotation = Quaternion.LookRotation(cameraForward);
        firePoint.rotation = Quaternion.LookRotation(cameraForward);

        // Apply sway and breathing adjustments on top of camera alignment
        ApplySway();
        ApplyBreathing();
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

        gunTransform.localPosition = initialGunPosition + new Vector3(swayX, swayY, 0f);
        gunTransform.localRotation = initialGunRotation * Quaternion.Euler(swayY, swayX, 0f);
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

    void Aim()
    {
        gunTransform.localPosition = Vector3.Lerp(gunTransform.localPosition, aimPosition, Time.deltaTime * aimSpeed);
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, aimFOV, Time.deltaTime * aimSpeed);
    }

    void ResetAim()
    {
        gunTransform.localPosition = Vector3.Lerp(gunTransform.localPosition, initialGunPosition, Time.deltaTime * aimSpeed);
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, initialCameraFOV, Time.deltaTime * aimSpeed);
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.linearVelocity = firePoint.forward * projectileSpeed;

        Projectile projScript = projectile.GetComponent<Projectile>();
        if (projScript != null)
        {
            projScript.SetDamage(damage);
        }
    }
}
