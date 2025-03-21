using UnityEngine;

public class SkyboxShake : MonoBehaviour
{
    public float shakeDuration = 1f; // Total duration of the shake
    public float shakeIntensity = 10f; // Intensity of rotation shake

    private float shakeElapsed = 0f;
    private bool isShaking = false;

    public void TriggerSkyboxShake()
    {
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
                // Calculate random rotation offsets based on intensity
                float rotationX = Random.Range(-shakeIntensity, shakeIntensity);
                float rotationY = Random.Range(-shakeIntensity, shakeIntensity);

                // Apply rotation to skybox
                RenderSettings.skybox.SetFloat("_Rotation", rotationX + rotationY);
            }
            else
            {
                // Stop shaking after the duration
                isShaking = false;
                RenderSettings.skybox.SetFloat("_Rotation", 0);
            }
        }
    }
}
