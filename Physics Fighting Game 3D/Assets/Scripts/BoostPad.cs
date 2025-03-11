using UnityEngine;
using System.Collections;

public class BoostPad : MonoBehaviour
{
    public float boostForce = 20f;
    public bool maintainVerticalVelocity = true; // If true, keeps player's vertical momentum
    public float cooldownTime = 1f;
    private bool canBoost = true;
    public GameObject boostEffectPrefab; // Change to prefab reference
    public AudioClip boostSound;

    private IEnumerator ResetBoost()
    {
        canBoost = false;
        yield return new WaitForSeconds(cooldownTime);
        canBoost = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canBoost) return;
        // Check if it's the player
        if (other.TryGetComponent<WASDMovement>(out WASDMovement player))
        {
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Get the boost direction from the pad's forward direction
                Vector3 boostDirection = transform.forward;
                
                // Optionally maintain the player's vertical velocity
                float verticalVelocity = maintainVerticalVelocity ? rb.linearVelocity.y : 0f;

                // Apply the boost
                rb.AddForce(boostDirection * boostForce, ForceMode.Impulse);
                
                // Spawn particle effect
                if (boostEffectPrefab != null)
                {
                    GameObject effect = Instantiate(boostEffectPrefab, transform.position, transform.rotation);
                    Destroy(effect, 2f); // Clean up after 2 seconds
                }
                
                if (boostSound != null) AudioSource.PlayClipAtPoint(boostSound, transform.position);
                StartCoroutine(ResetBoost());
            }
        }
    }
} 