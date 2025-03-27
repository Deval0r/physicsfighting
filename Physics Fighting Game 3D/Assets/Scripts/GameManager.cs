using UnityEngine;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public float money = 1000;
    private float maxMoney = 1500;  // Starting max (one bank worth)
    
    public float power = 1000;      // Current power
    private float maxPower = 1000;  // Starting max (one battery worth)
    
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI powerText;
    
    private float timer = 0f;
    public PlaceBuilding buildingManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buildingManager = FindObjectOfType<PlaceBuilding>();
        money = 4000;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            timer = 0f;
            UpdateResources();
        }

        // Update UI
        if (moneyText != null)
            moneyText.text = $"Money: {Mathf.Floor(money)}/{maxMoney}";
        if (powerText != null)
            powerText.text = $"Power: {Mathf.Floor(power)}/{maxPower}";
    }

    void UpdateResources()
    {
        // Calculate total power consumption
        float powerNeeded = 0;
        foreach (BuildingData building in buildingManager.buildings)
        {
            if (building.buildingName.ToLower() == "factory")
            {
                // Count how many of this type exist and multiply by power consumption
                // This assumes the name contains "Factory"
                GameObject[] factories = GameObject.FindGameObjectsWithTag("Building")
                    .Where(obj => obj.name.ToLower().Contains("factory")).ToArray();
                powerNeeded += factories.Length * 5f; // 5 power per factory
            }
        }

        // Only generate income if we have enough power
        if (power >= powerNeeded)
        {
            // Deduct power
            power = Mathf.Max(0, power - powerNeeded);

            // Add income from factories
            GameObject[] factories = GameObject.FindGameObjectsWithTag("Building")
                .Where(obj => obj.name.ToLower().Contains("factory")).ToArray();
            money = Mathf.Min(money + (factories.Length * 10), maxMoney);
        }
    }

    public void UpdateMaxMoney(float newMax)
    {
        maxMoney = Mathf.Max(1500, newMax); // Minimum of 1500
        money = Mathf.Min(money, maxMoney); // Cap current money if needed
    }

    public void UpdateMaxPower(float newMax)
    {
        maxPower = Mathf.Max(1000, newMax); // Minimum of 1000
        power = Mathf.Min(power, maxPower); // Cap current power if needed
        Debug.Log($"Max power updated to: {maxPower}"); // Add debug log to verify
    }
}
