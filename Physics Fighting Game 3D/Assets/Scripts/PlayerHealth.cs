using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Slider healthSlider; // Reference to the UI Slider

    void Start()
    {
        currentHealth = maxHealth; // Initialize player health

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth; // Set the Slider's max value
            healthSlider.value = currentHealth; // Set the Slider's current value
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health stays within bounds

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth; // Update the Slider value
        }

        Debug.Log($"Player took {damageAmount} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player is dead!");
        // Add logic for player death (e.g., restart level, show game over screen, etc.)
    }
}
