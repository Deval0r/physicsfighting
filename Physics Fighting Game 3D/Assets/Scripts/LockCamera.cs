using UnityEngine;

public class LockCameraRotation : MonoBehaviour
{
    public Transform player;    // Reference to the player's transform
    private Vector3 offset;     // Distance between camera and player
    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.rotation;
        // Store initial offset between camera and player
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // Update position to maintain offset from player
        transform.position = player.position + offset;
        transform.rotation = initialRotation;
    }
}