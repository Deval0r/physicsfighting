using UnityEngine;

public class ForcePlayAnimation : MonoBehaviour
{
    private Animation animationComponent;

    void Start()
    {
        // Get the Animation Component attached to this GameObject
        animationComponent = GetComponent<Animation>();

        if (animationComponent != null)
        {
            // Force play the default animation clip
            animationComponent.Play();
            Debug.Log("Animation forcefully started!");
        }
        else
        {
            Debug.LogWarning("No Animation component found on this GameObject!");
        }
    }
}
