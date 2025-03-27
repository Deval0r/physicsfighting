using UnityEngine;

public class SimpleCar : MonoBehaviour
{
    public float speed = 5f; // Default starting speed of the car
    public float travelDistance = 20f; // Distance the car will travel
    private Vector3 startPosition; // Starting position of the car
    private float traveled = 0f; // Tracks distance traveled

    private Material skyboxMaterial; // Reference to the skybox material
    private Color lastSkyboxColor; // Tracks the previous skybox color

    private Rigidbody rb; // Reference to the Rigidbody component

    private float elapsedTime = 0f; // Tracks time since the game started
    private bool allowSkyboxChangeDetection = false; // Flag for skybox change detection

    public AudioSource engineSoundSource; // Reference to the AudioSource for the engine sound
    public AudioClip engineSound; // Car engine sound clip
    [Range(0f, 5f)] public float engineVolume = 1f; // Volume control for the engine sound (0-5x boost)

    void Start()
    {
        // Save the car's starting position
        startPosition = transform.position;

        // Get the skybox material
        skyboxMaterial = RenderSettings.skybox;

        // Save the initial skybox color if it has a _Tint property
        if (skyboxMaterial.HasProperty("_Tint"))
        {
            lastSkyboxColor = skyboxMaterial.GetColor("_Tint");
        }

        // Get the Rigidbody component and ensure it is disabled initially
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Disable physics initially
        }

        // Set up the AudioSource for the engine sound
        if (engineSoundSource == null)
        {
            Debug.LogError("Engine Sound Source is not assigned in the Inspector!");
            return;
        }
        
        if (engineSound == null)
        {
            Debug.LogError("Engine Sound clip is not assigned in the Inspector!");
            return;
        }

        engineSoundSource.clip = engineSound;
        engineSoundSource.loop = true; // Loop the engine sound
        engineSoundSource.spatialBlend = 1.0f; // Enable 3D sound
        engineSoundSource.volume = engineVolume; // Set the initial volume
        engineSoundSource.Play(); // Start playing the engine sound
        
        Debug.Log($"Engine sound initialized - Volume: {engineVolume}, Is Playing: {engineSoundSource.isPlaying}");
    }

    void Update()
    {
        // Update the elapsed time
        elapsedTime += Time.deltaTime;

        // Allow skybox change detection only after 10 seconds
        if (!allowSkyboxChangeDetection && elapsedTime >= 10f)
        {
            allowSkyboxChangeDetection = true;
            Debug.Log("Skybox change detection enabled after 10 seconds.");
        }

        // Check if the skybox color has changed, but only if detection is enabled
        if (allowSkyboxChangeDetection && skyboxMaterial != null && skyboxMaterial.HasProperty("_Tint"))
        {
            Color currentColor = skyboxMaterial.GetColor("_Tint");
            if (currentColor != lastSkyboxColor)
            {
                // Skybox has changed - set speed to 150
                speed = 150f;

                // Update the last tracked color
                lastSkyboxColor = currentColor;

                Debug.Log("Skybox changed! Speed set to 150.");
            }
        }

        // Move the car in the opposite direction of its forward vector
        float step = speed * Time.deltaTime;
        transform.position -= transform.forward * step; // Use negative forward direction
        traveled += step;

        // Teleport the car back to the starting position if it exceeds the travel distance
        if (traveled >= travelDistance)
        {
            transform.position = startPosition;
            traveled = 0f; // Reset traveled distance
        }

        // Adjust engine sound pitch based on speed
        if (engineSoundSource != null)
        {
            engineSoundSource.pitch = Mathf.Clamp(speed / 50f, 0.5f, 2f); // Scale pitch by speed
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the object collided has the "Player" tag and speed is 150
        if (collision.collider.CompareTag("Player") && speed == 150f)
        {
            // Enable the Rigidbody component (enable physics)
            if (rb != null)
            {
                rb.isKinematic = false; // Enable physics
                Debug.Log("Rigidbody enabled after collision with Player!");
            }
        }
    }

    void OnDrawGizmos()
    {
        // Draw a line in the editor to represent the travel distance in the opposite direction
        Gizmos.color = Color.red;
        if (!Application.isPlaying) // Only draw the initial starting line in Edit mode
        {
            Gizmos.DrawLine(transform.position, transform.position - transform.forward * travelDistance);
        }
        else // In Play mode, reflect the initial starting point
        {
            Gizmos.DrawLine(startPosition, startPosition - transform.forward * travelDistance);
        }
    }
}
