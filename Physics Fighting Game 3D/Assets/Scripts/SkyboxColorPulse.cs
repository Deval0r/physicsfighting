using UnityEngine;

public class SkyboxColorPulse : MonoBehaviour
{
    public Gradient colorGradient; // Gradient for the pulse effect
    public Color initialTintColor = Color.white; // Initial tint color at scene start
    public float tintStrength = 0.5f; // Strength of the tint (0 = no tint, 1 = full override)
    public float duration = 1f; // Duration of the pulse effect
    private float elapsedTime = 0f; // Tracks elapsed time during the pulse
    private bool isPulsing = false; // Indicates if the pulse is active

    void Start()
    {
        // Apply the initial tint color blended with the texture when the scene starts
        if (RenderSettings.skybox.HasProperty("_Tint"))
        {
            RenderSettings.skybox.SetColor("_Tint", initialTintColor * tintStrength); // Blend tint with texture
        }
        else
        {
            Debug.LogWarning("Skybox material does not have a '_Tint' property. Ensure you are using a compatible skybox shader.");
        }
    }

    public void TriggerSkyboxPulse()
    {
        // Reset the elapsed time and begin the pulse without reverting the tint
        elapsedTime = 0f;
        isPulsing = true;
    }

    void Update()
    {
        if (isPulsing)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime < duration)
            {
                // Calculate progress (0 to 1) during the pulse duration
                float progress = elapsedTime / duration;

                // Blend the pulse gradient color with the texture using the gradient
                if (RenderSettings.skybox.HasProperty("_Tint"))
                {
                    RenderSettings.skybox.SetColor("_Tint", colorGradient.Evaluate(progress) * tintStrength);
                }
            }
            else
            {
                // End the pulse without reverting to the initial color
                isPulsing = false;
            }
        }
    }
}
