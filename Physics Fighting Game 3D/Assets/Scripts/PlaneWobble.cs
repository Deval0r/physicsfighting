using UnityEngine;

public class PlaneWobble : MonoBehaviour
{
    public float amplitude = 0.5f;  // Controls the intensity of the wobble
    public float frequency = 1.0f; // Controls the speed of the wobble

    private Vector3 initialPosition;

    void Start()
    {
        // Save the initial position of the plane
        initialPosition = transform.position;
    }

    void Update()
    {
        // Calculate wobble using sine and cosine waves
        float wobbleX = Mathf.Sin(Time.time * frequency) * amplitude;
        float wobbleZ = Mathf.Cos(Time.time * frequency) * amplitude;

        // Update the plane's position
        transform.position = initialPosition + new Vector3(wobbleX, 0, wobbleZ);
    }
}