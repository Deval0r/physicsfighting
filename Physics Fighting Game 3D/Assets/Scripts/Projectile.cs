using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage;
    private Rigidbody rb;
    public float maxRange = 100f; // New variable to specify max range
    private Vector3 initialPosition; // Variable to store the initial position

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = false; // Ensure gravity is off if you want the projectile to move in a straight line
        initialPosition = transform.position; // Store the initial position
    }

    public void SetDamage(int damageAmount)
    {
        damage = damageAmount;
    }

    void Update()
    {
        // Calculate the distance traveled from the initial position
        float distanceTraveled = Vector3.Distance(initialPosition, transform.position);

        // Destroy the projectile if it exceeds the max range
        if (distanceTraveled > maxRange)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
