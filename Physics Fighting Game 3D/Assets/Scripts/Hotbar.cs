using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    public PlaceBuilding placeBuilding;
    private int currentBuildingIndex;
    private RawImage factory;
    private RawImage battery;
    private RawImage bank;
    private RawImage farm;
    private RawImage windTurbine;
    private RawImage[] slots;
    private Color[] previousColors;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        placeBuilding = FindObjectOfType<PlaceBuilding>();
        // Get RawImage components from children
        factory = transform.GetChild(0).GetComponent<RawImage>();
        battery = transform.GetChild(1).GetComponent<RawImage>();
        bank = transform.GetChild(2).GetComponent<RawImage>();
        farm = transform.GetChild(3).GetComponent<RawImage>();
        windTurbine = transform.GetChild(4).GetComponent<RawImage>();
        
        // Store all slots in an array for easier access
        slots = new RawImage[] { factory, battery, bank, farm, windTurbine };
        previousColors = new Color[slots.Length];
        for (int i = 0; i < slots.Length; i++)
        {
            previousColors[i] = slots[i].color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Only update colors if the building index changed
        if (placeBuilding.buildingIndex != currentBuildingIndex)
        {
            // Reset previous selection
            if (currentBuildingIndex >= 0 && currentBuildingIndex < slots.Length)
            {
                slots[currentBuildingIndex].color = Color.white;
            }

            // Set new selection
            if (placeBuilding.buildingIndex >= 0 && placeBuilding.buildingIndex < slots.Length)
            {
                slots[placeBuilding.buildingIndex].color = Color.red;
            }

            currentBuildingIndex = placeBuilding.buildingIndex;
        }
    }
}
