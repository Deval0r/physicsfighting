using UnityEngine;
using TMPro;

public class PowerScript : MonoBehaviour
{
    private TextMeshProUGUI powerText;
    public GameManager gameManager;
    public PlaceBuilding placeBuilding;
    public int maxPower;
    public int currentPower;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        placeBuilding = FindObjectOfType<PlaceBuilding>();
        powerText = GetComponent<TextMeshProUGUI>(); 
    }
    void Update()
    {
        currentPower = gameManager.power;
        maxPower = placeBuilding.batteryCount * 1000;
        if (currentPower > maxPower)
        {
            gameManager.power = maxPower;
        }

        powerText.text = "Power: " + gameManager.power + "/" + maxPower + "J";
    }
}
