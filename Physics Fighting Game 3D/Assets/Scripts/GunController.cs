using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 20f;
    public int damage = 10;
    public Camera playerCamera; // Reference to the player camera
    public Transform gunTransform; // Reference to the gun transform

    void Update()
    {
        AimGun();

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void AimGun()
    {
        // Make the gun and firePoint align with the camera's forward direction
        Vector3 cameraForward = playerCamera.transform.forward;
        gunTransform.rotation = Quaternion.LookRotation(cameraForward);
        firePoint.rotation = Quaternion.LookRotation(cameraForward);
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
