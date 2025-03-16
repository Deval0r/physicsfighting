using UnityEngine;

public class NukeTrigger : MonoBehaviour
{
    public GameObject nukePrefab; // The nuke prefab to spawn
    public Transform nukeSpawnLocation; // The location where the nuke will spawn
    public AudioSource soundSource; // AudioSource to play the sound
    public AudioClip triggerSound; // The sound to play when the player touches the prefab
    private bool hasTriggered = false; // Ensures the event only happens once

    void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger has the "Player" tag
        if (!hasTriggered && other.CompareTag("Player"))
        {
        Debug.Log("Player has triggered the event!"); // Debug log
        hasTriggered = true;

        // Play the specified sound
        if (soundSource != null && triggerSound != null)
        {
            soundSource.PlayOneShot(triggerSound);
        }

        // Spawn the nuke at the specified location
        if (nukePrefab != null && nukeSpawnLocation != null)
        {
                Instantiate(nukePrefab, nukeSpawnLocation.position, nukeSpawnLocation.rotation);
            }
        }
    }
}
