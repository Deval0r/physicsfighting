using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public float detectionRadius = 10f;
    public float attackRadius = 2f;
    public float strafeDistance = 3f;
    public float decisionTime = 2f;
    public float turnSpeed = 5f; // New variable for turning speed

    private NavMeshAgent agent;
    private Transform player;
    private float decisionTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (health <= 0)
        {
            Die();
            return;
        }

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
        health -= damageAmount;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
