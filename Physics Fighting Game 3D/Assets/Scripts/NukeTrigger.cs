using UnityEngine;

public class NukeTrigger : MonoBehaviour
{
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

        // Automatically find SkyboxShake and SkyboxColorPulse if not assigned
        if (skyboxShake == null)
        {
            skyboxShake = FindObjectOfType<SkyboxShake>();
        }

        if (skyboxColorPulse == null)
        {
            skyboxColorPulse = FindObjectOfType<SkyboxColorPulse>();
        }

        // Deactivate the specified object at start
        if (objectToHide != null)
        {
            objectToHide.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger has the "Player" tag
        if (!hasTriggered && other.CompareTag("Player"))
        {
            Debug.Log("Player has triggered the event!"); // Debug log
            hasTriggered = true;

            // Activate the specified object when triggered
            if (objectToHide != null)
            {
                objectToHide.SetActive(true);
            }

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

            // Trigger camera shake
            if (cameraShake != null)
            {
                cameraShake.TriggerShake();
            }
            else
            {
                Debug.LogWarning("CameraShake script is missing! Please assign it to the Camera.");
            }

            // Trigger skybox shake
            if (skyboxShake != null)
            {
                skyboxShake.TriggerSkyboxShake();
            }
            else
            {
                Debug.LogWarning("SkyboxShake script is missing! Please ensure it's set up.");
            }

            // Trigger skybox color pulse
            if (skyboxColorPulse != null)
            {
                skyboxColorPulse.TriggerSkyboxPulse();
            }
            else
            {
                Debug.LogWarning("SkyboxColorPulse script is missing! Please ensure it's set up.");
            }
        }
    }
}
