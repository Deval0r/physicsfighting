using UnityEngine;

public class NukeTrigger : MonoBehaviour
{
    public delegate void NukeTriggered();
    public event NukeTriggered onNukeTriggered; // Event to notify when the nuke is triggered

    public GameObject nukePrefab; // The nuke prefab to spawn
    public Transform nukeSpawnLocation; // The location where the nuke will spawn
    public AudioSource soundSource; // AudioSource to play the sound
    public AudioClip triggerSound; // The sound to play when the player touches the prefab
    public CameraShake cameraShake; // Reference to the CameraShake script
    public SkyboxShake skyboxShake; // Reference to the SkyboxShake script
    public SkyboxColorPulse skyboxColorPulse; // Reference to the SkyboxColorPulse script
    public GameObject objectToHide; // Add this new variable
    private bool hasTriggered = false; // Ensures the event only happens once

    void Start()
    {
        // Find the CameraShake script if not manually assigned
        if (cameraShake == null && Camera.main != null)
        {
            cameraShake = Camera.main.GetComponent<CameraShake>();
        }

        if (skyboxShake == null)
        {
            skyboxShake = FindObjectOfType<SkyboxShake>();
        }

        if (skyboxColorPulse == null)
        {
            skyboxColorPulse = FindObjectOfType<SkyboxColorPulse>();
        }

        if (objectToHide != null)
        {
            objectToHide.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;

            if (objectToHide != null)
            {
                objectToHide.SetActive(true);
            }

            if (soundSource != null && triggerSound != null)
            {
                soundSource.PlayOneShot(triggerSound);
            }

            if (nukePrefab != null && nukeSpawnLocation != null)
            {
                Instantiate(nukePrefab, nukeSpawnLocation.position, nukeSpawnLocation.rotation);
            }

            if (cameraShake != null)
            {
                cameraShake.TriggerShake();
            }

            if (skyboxShake != null)
            {
                skyboxShake.TriggerSkyboxShake();
            }

            if (skyboxColorPulse != null)
            {
                skyboxColorPulse.TriggerSkyboxPulse();
            }

            // Notify any listeners that the nuke has been triggered
            onNukeTriggered?.Invoke();
        }
    }
}
