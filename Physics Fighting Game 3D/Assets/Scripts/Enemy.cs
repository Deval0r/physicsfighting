using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public float detectionRadius = 10f;
    public float attackRadius = 2f;
    public float strafeDistance = 3f;
    public float decisionTime = 2f;
    public float turnSpeed = 5f; // Variable for turning speed
    public float iframeDuration = 0.5f; // Duration of invincibility frames
    public GameObject floatingDamagePrefab; // Reference to the floating damage number prefab
    public Animator animator; // Reference to the Animator component

    private NavMeshAgent agent;
    private Transform player;
    private float decisionTimer;
    private float iframeTimer; // Timer to track iframe duration
    private bool isDead = false; // Flag to check if the enemy is already dead

    // Reference to the character control script
    private MonoBehaviour characterControlScript;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        characterControlScript = GetComponent<ZombieCharacterControl>(); // Replace with the actual script name

        // Debug log to check if floatingDamagePrefab is assigned
        if (floatingDamagePrefab == null)
        {
            Debug.LogError("Floating Damage Prefab is not assigned.");
        }
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

        iframeTimer -= Time.deltaTime; // Update iframe timer

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            agent.isStopped = false;

            if (distanceToPlayer <= attackRadius)
            {
                AttackPlayer();
            }
            else
            {
                PursuePlayer();
                FacePlayer(); // Ensure the enemy faces the player while pursuing
                decisionTimer -= Time.deltaTime;

                if (decisionTimer <= 0)
                {
                    decisionTimer = decisionTime;
                    MakeStrafeDecision();
                }
            }
        }
        else
        {
            agent.isStopped = true;
        }
    }

    void PursuePlayer()
    {
        agent.SetDestination(player.position);
    }

    void MakeStrafeDecision()
    {
        float strafeDirection = Random.value > 0.5f ? strafeDistance : -strafeDistance;
        Vector3 strafePosition = transform.position + transform.right * strafeDirection;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(strafePosition, out hit, strafeDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void AttackPlayer()
    {
        agent.isStopped = true;
        // Implement your attack logic here, e.g., dealing damage to the player
    }

    void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    public void TakeDamage(int damageAmount)
    {
        if (iframeTimer <= 0 && !isDead) // Only take damage if iframes have elapsed and not already dead
        {
            health -= damageAmount;
            iframeTimer = iframeDuration; // Reset iframe timer

            ShowFloatingDamage(damageAmount); // Show floating damage number

            if (health <= 0)
            {
                Die();
            }
        }
    }

    void ShowFloatingDamage(int damageAmount)
    {
        if (floatingDamagePrefab != null)
        {
            // Instantiate the damage number at the enemy's position with an upward offset
            Vector3 spawnPosition = transform.position + Vector3.up;
            GameObject damageNumber = Instantiate(floatingDamagePrefab, spawnPosition, Quaternion.identity);
            FloatingDamageNumber damageScript = damageNumber.GetComponent<FloatingDamageNumber>();

            // Debug log to check if damageScript is assigned
            if (damageScript == null)
            {
                Debug.LogError("FloatingDamageNumber component is not found on the instantiated prefab.");
            }
            else
            {
                damageScript.SetDamageText(damageAmount);
            }
        }
        else
        {
            Debug.LogError("Floating Damage Prefab is not assigned.");
        }
    }

    void Die()
    {
        isDead = true; // Set the death flag
        agent.isStopped = true; // Stop the agent from moving
        if (characterControlScript != null)
        {
            characterControlScript.enabled = false; // Disable the character control script
        }
        Debug.Log("Triggering DieTrigger");
        animator.SetTrigger("DieTrigger"); // Trigger the death animation
        StartCoroutine(WaitForDeathAnimation()); // Wait for the animation to finish
    }

    IEnumerator WaitForDeathAnimation()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        Debug.Log("Current Animator State: " + stateInfo.fullPathHash);
        yield return new WaitForSeconds(stateInfo.length);
        Destroy(gameObject); // Destroy the enemy after the death animation finishes
    }
}
