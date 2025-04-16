using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public GameObject objectToHide; // The object to hide
    public GameObject objectToActivate; // The object to activate when triggered
    public string playerTag = "Player"; // The tag used to identify the player

    private bool isPlayerNearby = false; // Tracks if the player is within interaction range

    void Update()
    {
        // Check if the player is nearby and presses the "F" key
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            // Activate the specified object
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
                Debug.Log($"{objectToActivate.name} has been activated.");
            }

            // Hide the specified object
            if (objectToHide != null)
            {
                objectToHide.SetActive(false);
                Debug.Log($"{objectToHide.name} has been hidden.");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the player tag
        if (other.CompareTag(playerTag))
        {
            isPlayerNearby = true; // Player is in range
            Debug.Log("Player is nearby. Press F to interact.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the object exiting the trigger has the player tag
        if (other.CompareTag(playerTag))
        {
            isPlayerNearby = false; // Player is out of range
            Debug.Log("Player left the interaction range.");
        }
    }
}
