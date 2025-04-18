using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public float currentHealth;

    public Slider healthSlider; // Reference to the UI Slider
    public float healthDrainTime = 150f; // Time in seconds for health to reach zero

    public void Start()
    {
        // If the health slider is not assigned in the Inspector, find it by tag
        if (healthSlider == null)
        {
            GameObject sliderObject = GameObject.FindWithTag("HealthSlider");
            if (sliderObject != null)
            {
                healthSlider = sliderObject.GetComponent<Slider>();
            }
            else
            {
                Debug.LogWarning("Health slider not found! Make sure it is tagged as 'HealthSlider'.");
            }
        }

        currentHealth = maxHealth; // Initialize player health

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth; // Set the Slider's max value
            healthSlider.value = currentHealth; // Set the Slider's current value
        }
    }

    void Update()
    {
        // Gradual health reduction over time
        float healthDrainRate = maxHealth / healthDrainTime; // Health to reduce per second
        currentHealth -= healthDrainRate * Time.deltaTime;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health stays within bounds

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth; // Update the Slider value
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        Debug.Log($"Player took {damageAmount} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public int GetCurrentHealth()
    {
        return Mathf.CeilToInt(currentHealth);
    }

    public void SetCurrentHealth(int health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth; // Update the Slider value
        }
    }

    void Die()
    {
        Debug.Log("Player is dead!");
        SceneManager.LoadScene("GameOver"); // Replace with your desired scene name
    }
}