using UnityEngine;

[System.Serializable]
public class BuildingData
{
    public string buildingName;
    public GameObject buildingPrefab;
    public int placeCost;
    public int removeCost;
    public Sprite buildingIcon;
    public float incomePerSecond;
    public float powerConsumption;  // Power used per second (negative for consumers, positive for generators)
} 