using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 1f; // Total duration of the shake
    public float shakeIntensity = 1f; // Maximum intensity of the shake
    public string growthMethod = "linear"; // Options: "linear", "sine", "exponential"

    private Vector3 originalLocalPosition; // To track the camera's original local position
    private float shakeElapsed = 0f; // Time elapsed during the shake
    private bool isShaking = false;

    void Start()
    {
        // Save the original local position of the camera
        originalLocalPosition = transform.localPosition;
    }

    public void TriggerShake()
    {
        // Reset the shake timer and flag
        shakeElapsed = 0f;
        isShaking = true;
    }

    void Update()
    {
        if (isShaking)
        {
            shakeElapsed += Time.deltaTime;

            if (shakeElapsed < shakeDuration)
            {
                // Calculate intensity based on the selected growth method
                float currentIntensity = CalculateIntensity(shakeElapsed / shakeDuration) * shakeIntensity;

                // Generate a random offset for the camera shake
                Vector3 randomOffset = new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f),
                    0f // Keep Z-axis stable since it's a child camera
                ) * currentIntensity;

                // Apply the offset to the camera's local position
                transform.localPosition = originalLocalPosition + randomOffset;
            }
            else
            {
                // Shake duration is complete, reset position and stop shaking
                isShaking = false;
                transform.localPosition = originalLocalPosition;
            }
        }
    }

    float CalculateIntensity(float progress)
    {
        switch (growthMethod.ToLower())
        {
            case "sine":
                return Mathf.Sin(progress * Mathf.PI); // Peaks at the middle of the duration
            case "linear":
                return progress; // Grows linearly over time
            case "exponential":
                return Mathf.Pow(progress, 2); // Intensity grows exponentially
            default:
                return 1f; // Default: constant intensity
        }
    }
}
