using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    public float rotationSpeed = 360f; // Degrees per second
    public float scaleSpeed = 5f; // Speed of scale lerping
    public float selectedScale = 1.2f; // Scale when selected
    private float currentRotation = 0f;
    private bool isRotating = false;
    private RawImage image;
    private Color previousColor;
    private Vector3 originalScale;
    private Vector3 targetScale;

    void Start()
    {
        image = GetComponent<RawImage>();
        previousColor = image.color;
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        // Check if color changed to red (selected)
        if (image.color == Color.red && previousColor != Color.red)
        {
            isRotating = true;
            currentRotation = 0f;
            targetScale = originalScale * selectedScale;
        }
        // Check if color changed from red (deselected)
        else if (image.color != Color.red && previousColor == Color.red)
        {
            targetScale = originalScale;
        }

        // Handle rotation
        if (isRotating)
        {
            currentRotation += rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, currentRotation, 0);

            if (currentRotation >= 360f)
            {
                isRotating = false;
                currentRotation = 0f;
                transform.rotation = Quaternion.identity;
            }
        }

        // Handle scaling
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);

        previousColor = image.color;
    }
}
