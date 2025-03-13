using UnityEngine;

public class UIFadeController : MonoBehaviour
{
    public CanvasGroup uiElement; // Reference to the CanvasGroup component
    public FPPWASDMovement playerMovement; // Reference to the player's movement script
    public float fadeSpeed = 2f; // Speed at which the UI fades in and out

    private void Update()
    {
        if (playerMovement != null && uiElement != null)
        {
            float targetAlpha = playerMovement.IsSprinting ? 1f : 0f; // Target alpha based on sprinting
            uiElement.alpha = Mathf.Lerp(uiElement.alpha, targetAlpha, Time.deltaTime * fadeSpeed); // Smoothly interpolate alpha
        }
    }
}
