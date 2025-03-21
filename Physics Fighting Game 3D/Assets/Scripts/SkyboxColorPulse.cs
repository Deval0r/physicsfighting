using UnityEngine;

public class SkyboxColorPulse : MonoBehaviour
{
    public Gradient colorGradient; // Define a gradient for the skybox color
    public float duration = 1f; // How long the pulse lasts
    private float elapsedTime = 0f;

    private bool isPulsing = false;

    public void TriggerSkyboxPulse()
    {
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
                // Modify skybox color based on gradient
                float progress = elapsedTime / duration;
                RenderSettings.skybox.SetColor("_Tint", colorGradient.Evaluate(progress));
            }
            else
            {
                isPulsing = false; // Stop pulsing
            }
        }
    }
}
