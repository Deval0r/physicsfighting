using UnityEngine;
using UnityEngine.UI;

public class ScreenFlashManager : MonoBehaviour
{
    private RawImage screenFlashImage; // Reference to the Raw Image
    public float flashDuration = 5f; // Duration of the screen flash fade-out

    void Start()
    {
        // Find the Raw Image on this object or its children
        screenFlashImage = GetComponentInChildren<RawImage>();
        if (screenFlashImage == null)
        {
            Debug.LogError("No Raw Image found for ScreenFlashManager!");
        }
    }

    public void TriggerFlash()
    {
        if (screenFlashImage != null)
        {
            StartCoroutine(FlashScreen());
        }
        else
        {
            Debug.LogError("Cannot start flash. Raw Image is missing!");
        }
    }

    private System.Collections.IEnumerator FlashScreen()
    {
        if (screenFlashImage == null)
        {
            yield break;
        }

        Debug.Log("FlashScreen coroutine started!");

        Color originalColor = new Color(1f, 1f, 1f, 0f); // Fully transparent white
        Color flashColor = new Color(1f, 1f, 1f, 1f);    // Fully opaque white

        // Step 1: Set the image to full opacity
        screenFlashImage.color = flashColor;
        Debug.Log("ScreenFlash set to full opacity.");

        // Step 2: Wait briefly for the flash effect
        yield return new WaitForSeconds(0.1f); // Optional delay to emphasize the flash effect

        // Step 3: Fade out the flash over the specified duration
        float elapsedTime = 0f;
        while (elapsedTime < flashDuration)
        {
            elapsedTime += Time.deltaTime;
            screenFlashImage.color = Color.Lerp(flashColor, originalColor, elapsedTime / flashDuration);
            Debug.Log($"Fading out. Alpha: {screenFlashImage.color.a}");
            yield return null;
        }

        // Step 4: Ensure the image is fully transparent at the end
        screenFlashImage.color = originalColor;
        Debug.Log("FlashScreen coroutine completed. ScreenFlash fully faded out.");
    }
}
