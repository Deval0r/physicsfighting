using UnityEngine;

public class Food : MonoBehaviour
{
    public PlayerHealth playerHealth; // Reference to the PlayerHealth script
    private float currentHeathThisScript; // Current health of this script
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
    }
    void OnTriggerEnter(Collider other)
    {
        print("Collision detected with: " + other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            playerHealth.currentHealth = 100;
        }
    }
}
