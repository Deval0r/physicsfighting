using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int money;
    public PlaceBuilding placeBuilding;

    private float moneyCooldown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        placeBuilding = FindObjectOfType<PlaceBuilding>();
        money = 1000;
        moneyCooldown = 3f; 
    }

    // Update is called once per frame
    void Update()
    {
        moneyCooldown -= Time.deltaTime;
        if (moneyCooldown <= 0)
        {
            money += placeBuilding.buildingCount * 10;
            moneyCooldown = 3f;
        }
    }
}
