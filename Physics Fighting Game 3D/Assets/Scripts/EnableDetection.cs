using UnityEngine;

public class EnableDetection : MonoBehaviour
{
    public GameObject secondaryObject; // The object to enable and later disable when the nuke triggers
    public NukeTrigger nukeTriggerScript; // Reference to the NukeTrigger script

    private bool isNukeTriggered = false; // Tracks if the nuke has been triggered

    void OnEnable()
    {
        // Check if the secondary object and NukeTrigger script are assigned
        if (secondaryObject != null && nukeTriggerScript != null)
        {
            // Enable the secondary object
            secondaryObject.SetActive(true);
            Debug.Log($"{gameObject.name} enabled, activating {secondaryObject.name}.");
            
            // Subscribe to the NukeTrigger event
            nukeTriggerScript.onNukeTriggered += DisableSecondaryObject;
        }
        else
        {
            Debug.LogWarning("Secondary object or NukeTrigger script not assigned.");
        }
    }

    void OnDisable()
    {
        // Unsubscribe from the NukeTrigger event to avoid errors
        if (nukeTriggerScript != null)
        {
            nukeTriggerScript.onNukeTriggered -= DisableSecondaryObject;
        }
    }

    void DisableSecondaryObject()
    {
        // Disable the secondary object when the nuke is triggered
        if (secondaryObject != null)
        {
            secondaryObject.SetActive(false);
            Debug.Log($"{secondaryObject.name} disabled as nuke was triggered.");
        }

        isNukeTriggered = true; // Update state
    }
}
