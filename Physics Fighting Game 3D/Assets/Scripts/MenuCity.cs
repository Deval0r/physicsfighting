using UnityEngine;

public class MouseRotation : MonoBehaviour
{
    private Camera mainCamera;
    public float maxRotation = 45f; // Maximum rotation angle in degrees
    public float lerpSpeed = 5f; // Speed of rotation lerping
    private float targetRotation;
    private float currentViewportX; // Track the current viewport position
    private float currentRotation; // Track the current rotation
    public float oscillationSpeed = 1f; // Speed of the sine wave oscillation
    private float timeElapsed; // Track time for sine wave

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        currentViewportX = 0.5f; // Start at center
        currentRotation = 0f; // Start at neutral rotation
        timeElapsed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Update time for sine wave
        timeElapsed += Time.deltaTime * oscillationSpeed;
        
        // Calculate X rotation using sine wave (-220 to -140)
        float xRotation = -180f + (Mathf.Sin(timeElapsed) * 40f);
        
        // Get mouse position in screen coordinates
        Vector3 mousePos = Input.mousePosition;
        
        // Convert to viewport coordinates (0 to 1)
        float viewportX = mousePos.x / Screen.width;
        
        // Smoothly interpolate the viewport position with reduced sensitivity
        currentViewportX = Mathf.Lerp(currentViewportX, viewportX, Time.deltaTime * lerpSpeed * 0.5f);
        
        // Convert viewport position to rotation angle with reduced range
        targetRotation = (currentViewportX - 0.5f) * maxRotation;
        
        // Smoothly interpolate the rotation
        currentRotation = Mathf.Lerp(currentRotation, targetRotation, Time.deltaTime * lerpSpeed * 0.5f);
        
        // Apply the rotation with oscillating X rotation
        transform.rotation = Quaternion.Euler(xRotation, 0, currentRotation);
    }
}
