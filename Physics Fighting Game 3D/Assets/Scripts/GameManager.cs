using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int money;
    public int power;
    public PlaceBuilding placeBuilding;
    private float Cooldown;

    public GameObject targetObject; // The target object to monitor
    public GameObject player; // The player GameObject
    private PlayerHealth gameManagerHealthScript;

    void Start()
    {
        placeBuilding = FindObjectOfType<PlaceBuilding>();
        money = 5000;
        power = 100;
        Cooldown = 3f;

        // Get the PlayerHealth script attached to the GameManager
        gameManagerHealthScript = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        Cooldown -= Time.deltaTime;
        if (Cooldown <= 0)
        {
            money += placeBuilding.factoryCount * 10;
            power = (placeBuilding.windTurbineCount * 10) - (placeBuilding.factoryCount * 5 + placeBuilding.bankCount * 5);
            if (power < 0)
            {
                power = 0;
                money -= placeBuilding.factoryCount * 20;
            }

            Cooldown = 3f;
        }

        // Check if the target object becomes active
        if (targetObject.activeSelf && gameManagerHealthScript != null)
        {
            TransferHealthToPlayer();
        }
    }

    void TransferHealthToPlayer()
    {
        // Ensure the player has a PlayerHealth component
        PlayerHealth playerHealthScript = player.GetComponent<PlayerHealth>();
        if (playerHealthScript == null)
        {
            // Dynamically add PlayerHealth to the Player if it doesn't already exist
            playerHealthScript = player.AddComponent<PlayerHealth>();
        }

        // Transfer health values and settings from the GameManager's PlayerHealth
        playerHealthScript.maxHealth = gameManagerHealthScript.maxHealth;
        playerHealthScript.SetCurrentHealth(gameManagerHealthScript.GetCurrentHealth());

        Debug.Log("Health successfully transferred from GameManager to Player!");

        // Optionally disable the GameManager's PlayerHealth script to avoid duplication
        gameManagerHealthScript.enabled = false;

        // Reassign the health slider after transfer
        playerHealthScript.Start(); // Re-run Start to initialize the health slider
    }
}