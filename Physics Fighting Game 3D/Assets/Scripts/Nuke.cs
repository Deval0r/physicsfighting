using UnityEngine;
using UnityEngine.UI;

public class Nuke : MonoBehaviour
{
    public GameObject[] zombieSpawnerPrefabs; // Array of zombie spawner prefabs
    public Transform[] spawnLocations; // Array of spawn locations for the spawners
    public AudioClip nukeSound; // Nuke sound effect
    public Image screenFlashImage; // UI Image for the screen flash
    public float flashDuration = 4f; // Duration of the screen flash

    private AudioSource audioSource;

    void Start()
    {
        // Add an AudioSource component if not already present
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Trigger the nuke effects when it hits the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            TriggerNukeEffects();
        }
    }

    void TriggerNukeEffects()
    {
        // Spawn zombie spawners at specified locations
        foreach (Transform location in spawnLocations)
        {
            int randomIndex = Random.Range(0, zombieSpawnerPrefabs.Length);
            Instantiate(zombieSpawnerPrefabs[randomIndex], location.position, Quaternion.identity);
        }

        // Play the nuke sound
        if (nukeSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(nukeSound);
        }

        // Start the screen flash effect
        if (screenFlashImage != null)
        {
            StartCoroutine(FlashScreen());
        }

        // Destroy the nuke object
        Destroy(gameObject);
    }

    System.Collections.IEnumerator FlashScreen()
    {
        Color originalColor = screenFlashImage.color;
        Color flashColor = new Color(1f, 1f, 1f, 1f); // Full white

        float elapsedTime = 0f;

        // Fade in the flash
        while (elapsedTime < flashDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            screenFlashImage.color = Color.Lerp(originalColor, flashColor, elapsedTime / (flashDuration / 2));
            yield return null;
        }

        elapsedTime = 0f;

        // Fade out the flash
        while (elapsedTime < flashDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            screenFlashImage.color = Color.Lerp(flashColor, originalColor, elapsedTime / (flashDuration / 2));
            yield return null;
        }

        // Reset the flash color
        screenFlashImage.color = originalColor;
    }
}
