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
        Vector3 cameraForward = playerCamera.transform.forward;
        gunTransform.rotation = Quaternion.LookRotation(cameraForward);
        firePoint.rotation = Quaternion.LookRotation(cameraForward);
    }

    void ApplySway()
    {
        float moveX = Input.GetAxis("Horizontal") * (Input.GetButton("Fire2") ? aimSwayAmount : swayAmount);
        float moveY = Input.GetAxis("Vertical") * (Input.GetButton("Fire2") ? aimSwayAmount : swayAmount);

        swayTimer += Time.deltaTime * swaySpeed;
        float swayX = Mathf.Sin(swayTimer) * moveX;
        float swayY = Mathf.Sin(swayTimer) * moveY;

        gunTransform.localPosition = initialGunPosition + new Vector3(swayX, swayY, 0f);
        gunTransform.localRotation = initialGunRotation * Quaternion.Euler(swayY, swayX, 0f);
    }

    void ApplyBreathing()
    {
        breathingTimer += Time.deltaTime * breathingSpeed;
        float breathingOffset = Mathf.Sin(breathingTimer) * (Input.GetButton("Fire2") ? aimSwayAmount : breathingAmount);

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
