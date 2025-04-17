using UnityEngine;

public class TitleScreenRotator : MonoBehaviour
{
    public float maxYRotationSpeed = 200f; // Maximum speed of rotation on Y-axis at far angles (±45 degrees)
    public float minYRotationSpeed = 20f; // Minimum speed of rotation on Y-axis when facing forward (0 degrees)
    public float yOscillationAmplitude = 0.5f; // Maximum height difference for oscillation
    public float yOscillationSpeed = 2f; // Speed of up-and-down oscillation

    private float yOscillationTimer = 0f; // Tracks time for Y-position oscillation
    private Vector3 initialPosition; // Stores the object's initial position

    void Start()
    {
        // Store the initial position of the object
        initialPosition = transform.position;
    }

    void Update()
    {
        // Handle Y-axis rotation
        float yAngle = Mathf.DeltaAngle(0, transform.eulerAngles.y);
        float yRotationSpeed = Mathf.Lerp(minYRotationSpeed, maxYRotationSpeed, Mathf.Abs(yAngle) / 45f);
        transform.Rotate(Vector3.up * yRotationSpeed * Time.deltaTime);

        // Handle Y-position oscillation
        yOscillationTimer += Time.deltaTime * yOscillationSpeed;
        float yOffset = Mathf.Sin(yOscillationTimer) * yOscillationAmplitude; // Oscillates between -amplitude and +amplitude
        transform.position = new Vector3(initialPosition.x, initialPosition.y + yOffset, initialPosition.z);
    }
}
