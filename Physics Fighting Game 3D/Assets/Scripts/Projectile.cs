using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage;
    private Rigidbody rb;
    public float maxRange = 100f; // New variable to specify max range
    private Vector3 initialPosition; // Variable to store the initial position
    public AudioClip hitSound; // Variable for the hit sound
    private AudioSource audioSource; // Variable for the AudioSource

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = false; // Ensure gravity is off if you want the projectile to move in a straight line
        initialPosition = transform.position; // Store the initial position
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component

        // Check if AudioSource component is present
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing. Please add an AudioSource component to the projectile.");
        }
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

            // Play hit sound with slight pitch variation
            if (audioSource != null && hitSound != null)
            {
                rb.linearVelocity = Vector3.zero; // Freeze the projectile's position by stopping its movement
                rb.isKinematic = true; // Make the rigidbody kinematic to prevent further physics interactions

                audioSource.pitch = Random.Range(0.9f, 1.1f); // Random pitch variation between 0.9 and 1.1
                audioSource.PlayOneShot(hitSound);

                // Delay the destruction to allow the sound to play
                Destroy(gameObject, hitSound.length);
            }
            else
            {
                Debug.LogError("AudioSource or hitSound is not assigned.");
                Destroy(gameObject); // Ensure the projectile is destroyed if sound fails to play
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
