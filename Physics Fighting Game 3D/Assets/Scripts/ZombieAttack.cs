using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    public float attackRadius = 2f;         // Radius within which the zombie can attack
    public int attackDamage = 10;          // Damage dealt to the player per attack
    public float attackCooldown = 2f;      // Time in seconds between attacks
    public Animator animator;              // Reference to the Animator component
    public LayerMask playerLayer;          // Layer mask to identify the player

    public AudioClip[] attackSounds;       // Array to hold the attack sounds (2 sounds)
    private AudioSource audioSource;       // Reference to the AudioSource component

    private bool canAttack = true;         // Controls attack cooldown

    void Start()
    {
        // Initialize the AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Check if attack sounds are properly set up
        if (attackSounds == null || attackSounds.Length < 2)
        {
            Debug.LogError("Please assign at least 2 attack sounds to the ZombieAttack script!");
        }
    }

    void Update()
    {
        // Detect if the player is within the attack radius
        Collider[] playersInRange = Physics.OverlapSphere(transform.position, attackRadius, playerLayer);

        if (playersInRange.Length > 0) // If the player is in range
        {
            Transform player = playersInRange[0].transform;
            AttackPlayer(player);
        }
    }

    void AttackPlayer(Transform player)
    {
        if (canAttack)
        {
            // Trigger the attack animation
            animator.SetTrigger("Attack");

            // Delay the damage and attack sound by 0.3 seconds
            Invoke(nameof(PerformAttack), 0.3f);
            StartCoroutine(DelayedAttackSound(0.3f));

            // Start the cooldown
            StartCoroutine(AttackCooldown());
        }
    }

    void PerformAttack()
    {
        // Deal damage to the player
        Collider[] playersInRange = Physics.OverlapSphere(transform.position, attackRadius, playerLayer);
        foreach (Collider playerCollider in playersInRange)
        {
            PlayerHealth playerHealth = playerCollider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log($"Player took {attackDamage} damage. Current health: {playerHealth}");
            }
        }
    }

    System.Collections.IEnumerator DelayedAttackSound(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (attackSounds.Length > 0 && audioSource != null)
        {
            int randomIndex = Random.Range(0, attackSounds.Length); // Pick a random sound
            audioSource.clip = attackSounds[randomIndex];
            
            // Randomize the pitch by ±0.1
            audioSource.pitch = Random.Range(0.9f, 1.1f);

            audioSource.Play();
            Debug.Log($"Playing attack sound: {attackSounds[randomIndex].name} with pitch: {audioSource.pitch}");
        }
        else
        {
            Debug.LogError("Attack sounds are not assigned or AudioSource is missing!");
        }
    }

    System.Collections.IEnumerator AttackCooldown()
    {
        canAttack = false; // Prevent attacking
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true; // Allow attacking again
    }

    void OnDrawGizmosSelected()
    {
        // Draw the attack radius in the Scene view for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
