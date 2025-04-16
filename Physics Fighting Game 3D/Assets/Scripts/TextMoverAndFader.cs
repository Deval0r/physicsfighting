using UnityEngine;
using UnityEngine.UI;

public class TextMoverAndFader : MonoBehaviour
{
    public float moveSpeed = 50f; // Initial speed of the text movement
    public float fadeSpeed = 1f; // Speed at which the text fades away
    public float stopPositionY = -100f; // Y position at which the text stops moving
    public float slowingDistance = 50f; // Distance over which the text slows down

    private Text textComponent; // The Text component on the canvas object
    private CanvasGroup canvasGroup; // For controlling opacity
    private bool isFading = false; // Tracks if fading has started

    void Start()
    {
        // Get the Text and CanvasGroup components
        textComponent = GetComponent<Text>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            // If there's no CanvasGroup, add one
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    void Update()
    {
        // Gradually slow down as the text approaches the stop position
        if (transform.localPosition.y > stopPositionY && !isFading)
        {
            float distanceToStop = Mathf.Abs(transform.localPosition.y - stopPositionY);
            float currentSpeed = Mathf.Lerp(0, moveSpeed, distanceToStop / slowingDistance);
            transform.localPosition -= new Vector3(0, currentSpeed * Time.deltaTime, 0);
        }
        else if (!isFading)
        {
            // Start fading when the text reaches the stop position
            isFading = true;
        }

        // Fade out the text
        if (isFading)
        {
            canvasGroup.alpha -= fadeSpeed * Time.deltaTime;

            // Destroy the game object when fully faded out
            if (canvasGroup.alpha <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
