using UnityEngine;

public class Hotbar : MonoBehaviour
{
    public PlaceBuilding placeBuilding;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        placeBuilding = FindObjectOfType<PlaceBuilding>();
    }

    // Update is called once per frame
    void Update()
    {
        print(placeBuilding.buildingIndex);
        Transform factory = transform.GetChild(0);
        Transform battery = transform.GetChild(1);
        Transform bank = transform.GetChild(2);
        if (placeBuilding.buildingIndex == 0)
        {
            factory.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        if (placeBuilding.buildingIndex == 1)
        {
            battery.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        if (placeBuilding.buildingIndex == 2)
        {
            bank.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }
}
