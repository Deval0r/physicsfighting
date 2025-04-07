using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    public PlaceBuilding placeBuilding;
    private int currentBuildingIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        placeBuilding = FindObjectOfType<PlaceBuilding>();
    }

    // Update is called once per frame
    void Update()
    {
        Transform factory = transform.GetChild(0);
        Transform battery = transform.GetChild(1);
        Transform bank = transform.GetChild(2);
        Transform farm = transform.GetChild(3);
        Transform windTurbine = transform.GetChild(4);

        if (placeBuilding.buildingIndex == 0)
        {
            factory.GetComponent<Image>().color = Color.red;
        }
        if (placeBuilding.buildingIndex == 1)
        {
            battery.GetComponent<Image>().color = Color.red;
        }
        if (placeBuilding.buildingIndex == 2)
        {
            bank.GetComponent<Image>().color = Color.red;
        }
        if (placeBuilding.buildingIndex == 3)
        {
            farm.GetComponent<Image>().color = Color.red;
        }
        if (placeBuilding.buildingIndex == 4)
        {
            windTurbine.GetComponent<Image>().color = Color.red;
        }
        if (placeBuilding.buildingIndex != currentBuildingIndex)
        {
            if (currentBuildingIndex == 0)
            {
                factory.GetComponent<Image>().color = Color.white;
            }
            if (currentBuildingIndex == 1)
            {
                battery.GetComponent<Image>().color = Color.white;
            }
            if (currentBuildingIndex == 2)
            {
                bank.GetComponent<Image>().color = Color.white;
            }
            if (currentBuildingIndex == 3)
            {
                farm.GetComponent<Image>().color = Color.white;
            }
            if (currentBuildingIndex == 4)
            {
                windTurbine.GetComponent<Image>().color = Color.white;
            }
            currentBuildingIndex = placeBuilding.buildingIndex;
        }
    }
}
