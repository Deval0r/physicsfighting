using UnityEngine;

public class HealthTransfer : MonoBehaviour
{
    public GameObject player; // Reference to the Player GameObject
    private PlayerHealth targetHealthScript;

    void OnEnable()
    {
        // Check if the PlayerHealth script exists on the target object
        targetHealthScript = GetComponent<PlayerHealth>();
        if (targetHealthScript != null && player != null)
        {
            TransferPlayerHealth();
        }
    }

    void TransferPlayerHealth()
    {
        // Check if the player already has a PlayerHealth component
        PlayerHealth playerHealthScript = player.GetComponent<PlayerHealth>();
        if (playerHealthScript == null)
        {
            // Add the PlayerHealth component to the player
            playerHealthScript = player.AddComponent<PlayerHealth>();
        }

        // Transfer relevant properties
        playerHealthScript.maxHealth = targetHealthScript.maxHealth;
        playerHealthScript.SetCurrentHealth(targetHealthScript.GetCurrentHealth());

        Debug.Log("PlayerHealth successfully transferred to the player!");

        // Optionally disable the original target script to avoid conflicts
        targetHealthScript.enabled = false;
    }
}