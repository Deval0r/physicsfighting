using UnityEngine;

public class Food : MonoBehaviour
{
    public PlayerHealth playerHealth; // Reference to the PlayerHealth script
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            playerHealth.currentHealth = 100; // Increase player health by 10
        }
    }
}
