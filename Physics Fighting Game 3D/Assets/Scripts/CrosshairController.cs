using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    public GameObject player; // Reference to the player
    public RawImage crosshair; // RawImage crosshair UI element

    public float normalSize = 50f; // Default crosshair size
    public float enlargedSize = 75f; // Size during sprinting/jumping
    public float normalTransparency = 1f; // Default transparency
    public float actionTransparency = 0.25f; // Transparency during sprint/jump (0.25 = 25%)
    public float resizeSpeed = 5f; // Speed of transition

    private FPPWASDMovement playerMovement; // Reference to the player's movement script
    private float targetSize; // Target size for the crosshair
    private float targetTransparency; // Target transparency for the crosshair

    void Start()
    {
        if (player != null)
        {
            playerMovement = player.GetComponent<FPPWASDMovement>();
            if (playerMovement == null)
            {
                Debug.LogError("PlayerMovement script not found!");
            }
        }

        if (crosshair == null)
        {
            Debug.LogError("Crosshair RawImage not assigned!");
        }

        // Set default values
        targetSize = normalSize;
        targetTransparency = normalTransparency;
    }

    void Update()
    {
        if (playerMovement != null && crosshair != null)
        {
            // Check if the player is sprinting OR jumping
            if (playerMovement.isSprinting || ! playerMovement.isGrounded)
            {
                targetSize = enlargedSize;
                targetTransparency = actionTransparency;
            }
            else
            {
                targetSize = normalSize;
                targetTransparency = normalTransparency;
            }

            // Smoothly interpolate size
            float newSize = Mathf.Lerp(crosshair.rectTransform.sizeDelta.x, targetSize, Time.deltaTime * resizeSpeed);
            crosshair.rectTransform.sizeDelta = new Vector2(newSize, newSize);

            // Smoothly interpolate transparency
            Color crosshairColor = crosshair.color;
            crosshairColor.a = Mathf.Lerp(crosshairColor.a, targetTransparency, Time.deltaTime * resizeSpeed);
            crosshair.color = crosshairColor;
        }
    }
}