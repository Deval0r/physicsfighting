using UnityEngine;
using TMPro;

public class PowerScript : MonoBehaviour
{
    private TextMeshProUGUI powerText;
    public GameManager gameManager;
    public PlaceBuilding placeBuilding;
    public int maxPower;
    public int currentPower;
    private float displayPower; // Display value for smooth lerping
    private float displayMaxPower; // Display value for max power
    public float lerpSpeed = 10f; // Speed of the lerp

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        placeBuilding = FindObjectOfType<PlaceBuilding>();
        powerText = GetComponent<TextMeshProUGUI>(); 
        displayPower = gameManager.power; // Initialize display value
        displayMaxPower = placeBuilding.batteryCount * 1000; // Initialize max display value
    }

    void Update()
    {
        currentPower = gameManager.power;
        maxPower = placeBuilding.batteryCount * 1000;
        if (currentPower > maxPower)
        {
            gameManager.power = maxPower;
        }

        // Smoothly lerp both display values
        displayPower = Mathf.Lerp(displayPower, currentPower, Time.deltaTime * lerpSpeed);
        displayMaxPower = Mathf.Lerp(displayMaxPower, maxPower, Time.deltaTime * lerpSpeed);

        powerText.text = "Power: " + Mathf.RoundToInt(displayPower) + "/" + Mathf.RoundToInt(displayMaxPower) + "J";
    }
}
