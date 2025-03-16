using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieProximitySound : MonoBehaviour
{
    public float detectionRadius = 10f; // Radius for detecting zombies
    public float soundCooldown = 10f;  // Time in seconds between sound plays
    public AudioClip[] zombieSounds;   // Array of 3 sounds to play (assign in the Inspector)
    public LayerMask zombieLayer;      // Layer mask to identify zombie objects
    public AudioSource audioSource;    // AudioSource component to play sounds

    private HashSet<Transform> encounteredZombies = new HashSet<Transform>(); // Track zombies already encountered
    private bool canPlaySound = true; // To control cooldown timing

    void Update()
    {
        // Detect zombies within the radius
        Collider[] zombiesInRange = Physics.OverlapSphere(transform.position, detectionRadius, zombieLayer);

        foreach (Collider zombie in zombiesInRange)
        {
            Transform zombieTransform = zombie.transform;

            // Skip already encountered zombies
            if (encounteredZombies.Contains(zombieTransform))
                continue;

            // Play a sound for a new zombie if allowed
            if (canPlaySound)
            {
                PlayRandomSound();
                encounteredZombies.Add(zombieTransform); // Mark this zombie as encountered
                StartCoroutine(SoundCooldown());
                break; // Only play one sound per update
            }
        }
    }

    void PlayRandomSound()
    {
        if (zombieSounds.Length > 0 && audioSource != null)
        {
            // Pick a random sound from the array
            int randomIndex = Random.Range(0, zombieSounds.Length);
            audioSource.clip = zombieSounds[randomIndex];
            audioSource.Play();
            Debug.Log($"Playing sound: {zombieSounds[randomIndex].name}");
        }
        else
        {
            Debug.LogError("Zombie sounds or AudioSource not assigned!");
        }
    }

    IEnumerator SoundCooldown()
    {
        canPlaySound = false; // Prevent playing sounds
        yield return new WaitForSeconds(soundCooldown);
        canPlaySound = true; // Re-enable sound playing
    }

    void OnDrawGizmosSelected()
    {
        // Draw the detection radius in the scene view for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
