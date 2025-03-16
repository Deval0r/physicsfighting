using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;   
    public AudioSource ambientAudioSource; // Separate AudioSource for ambient audio
    // Reference to the AudioSource component
    public AudioClip loopingClip;         // Audio clip to loop after the nuke hits
    public AudioClip zombieScreamsClip;   // Audio clip for zombie screams

    public float detectionRadius = 15f;  // Radius for detecting zombies
    public LayerMask zombieLayer;        // Layer mask to identify zombie objects
    public int zombiesForMaxVolume = 10; // Number of zombies required for maximum volume
    public float maxVolume = 1f;         // Maximum volume for screams
    public float soundCooldown = 10f;    // Cooldown between scream sounds in seconds

    private Transform player;            // Reference to the player
    private bool canPlayZombieScreams = true; // Controls cooldown for zombie screams

    void Start()
    {
        // Find player
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
        player = playerObject.transform;
    }
    else
    {
        Debug.LogError("Player not found! Make sure the player is tagged as 'Player'.");
    }

    // Assign primary AudioSource
    if (audioSource == null)
    {
        audioSource = GetComponent<AudioSource>();
    }
    if (audioSource == null)
    {
        Debug.LogError("No AudioSource found for zombie screams!");
    }

    // Assign ambient AudioSource
    if (ambientAudioSource == null)
    {
        ambientAudioSource = gameObject.AddComponent<AudioSource>(); // Add one if missing
    }
    if (ambientAudioSource == null)
    {
        Debug.LogError("No AudioSource found for ambient audio!");
    }
    }


    void Update()
    {
        if (canPlayZombieScreams && player != null)
        {
            // Detect zombies within the radius
            Collider[] zombiesInRange = Physics.OverlapSphere(player.position, detectionRadius, zombieLayer);

            // Get the number of zombies in range
            int zombieCount = zombiesInRange.Length;

            if (zombieCount > 0)
            {
                // Calculate volume based on the number of zombies
                float volume = Mathf.Clamp01((float)zombieCount / zombiesForMaxVolume) * maxVolume;

                // Play zombie screams at calculated volume
                PlayZombieScreams(volume);

                // Start cooldown for zombie screams
                StartCoroutine(ZombieScreamsCooldown());
            }
        }
    }

    void PlayZombieScreams(float volume)
    {
        if (audioSource != null && zombieScreamsClip != null)
        {
        // Reset volume
        float totalVolume = 0f;

        // Detect zombies within the radius
        Collider[] zombiesInRange = Physics.OverlapSphere(player.position, detectionRadius, zombieLayer);
        foreach (Collider zombie in zombiesInRange)
        {
            // Calculate distance and weight volume based on proximity
            float distance = Vector3.Distance(player.position, zombie.transform.position);
            float proximityFactor = 1 - (distance / detectionRadius); // Closer zombies contribute more
            totalVolume += Mathf.Clamp01(proximityFactor); // Ensure value is between 0 and 1
        }

        // Scale total volume by maxVolume and zombiesForMaxVolume
        float scaledVolume = Mathf.Clamp01(totalVolume / zombiesForMaxVolume) * maxVolume;

        // Play the sound with calculated volume
        audioSource.clip = zombieScreamsClip;
        audioSource.volume = scaledVolume;
        audioSource.Play();
        Debug.Log($"Playing zombie screams with volume: {scaledVolume}");
    }
    else
    {
        Debug.LogError("ZombieScreamsClip or AudioSource is not assigned.");
    }
    }


    IEnumerator ZombieScreamsCooldown()
    {
        canPlayZombieScreams = false; // Prevent sound during cooldown
        yield return new WaitForSeconds(soundCooldown); // Wait for the cooldown period
        canPlayZombieScreams = true; // Allow sounds again
    }

    public void PlayLoopingAudio()
    {
    if (ambientAudioSource != null && loopingClip != null)
    {
        ambientAudioSource.clip = loopingClip;
        ambientAudioSource.loop = true; // Enable looping
        ambientAudioSource.Play();
        Debug.Log("Ambient audio is now looping.");
    }
    else
    {
        Debug.LogError("AmbientAudioSource or LoopingClip not assigned!");
    }
    }


    void OnDrawGizmosSelected()
    {
        // Draw the detection radius in the Scene view for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player != null ? player.position : transform.position, detectionRadius);
    }
}
