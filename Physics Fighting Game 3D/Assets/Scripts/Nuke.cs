using UnityEngine;

public class Nuke : MonoBehaviour
{
    public GameObject[] zombieSpawnerPrefabs; // Array of zombie spawner prefabs
    public Transform[] spawnLocations; // Array of spawn locations for the spawners
    public float flashDuration = 5f; // Duration of the screen flash fade-out
    private Renderer nukeRenderer; // Reference to the nuke's Renderer
    private bool hasTriggered = false; // Ensures the effects only happen once

    private ScreenFlashManager screenFlashManager; // Reference to the ScreenFlashManager

    void Start()
    {
        // Find the ScreenFlashManager in the scene
        screenFlashManager = FindObjectOfType<ScreenFlashManager>();
        if (screenFlashManager == null)
        {
            Debug.LogError("No ScreenFlashManager found in the scene!");
        }

        // Get the nuke's Renderer component for visibility control
        nukeRenderer = GetComponent<Renderer>();
        if (nukeRenderer == null)
        {
            Debug.LogError("No Renderer found on the Nuke object. Visibility changes won't work!");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Trigger the nuke effects when it touches the ground
        if (!hasTriggered && collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Nuke hit the ground!");
            hasTriggered = true;

            // Signal the ScreenFlashManager to trigger the flash
            if (screenFlashManager != null)
            {
                screenFlashManager.TriggerFlash();
                Debug.Log("Flash triggered on ScreenFlashManager.");
            }
            else
            {
                Debug.LogError("ScreenFlashManager not found. Flash will not occur.");
            }

            // Make the nuke invisible immediately
            if (nukeRenderer != null)
            {
                Debug.Log("Disabling nuke renderer to make it invisible.");
                nukeRenderer.enabled = false;
            }
            else
            {
                Debug.LogError("Nuke renderer not found. Cannot make it invisible.");
            }

            // Trigger additional effects like spawning zombie spawners
            TriggerNukeEffects();

            // Destroy the nuke immediately after all actions
            Destroy(gameObject);
        }
    }

    void TriggerNukeEffects()
    {
        // Spawn zombie spawners at specified locations
        foreach (Transform location in spawnLocations)
        {
            int randomIndex = Random.Range(0, zombieSpawnerPrefabs.Length);
            Instantiate(zombieSpawnerPrefabs[randomIndex], location.position, Quaternion.identity);
            Debug.Log($"Zombie spawner spawned at {location.position}");
        }
    }
}
