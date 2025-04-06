using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int money;
    public int power;
    public PlaceBuilding placeBuilding;
    private float Cooldown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        placeBuilding = FindObjectOfType<PlaceBuilding>();
        money = 99999;
        power = 100;
        Cooldown = 3f; 
    }

    // Update is called once per frame
    void Update()
    {
        Cooldown -= Time.deltaTime;
        if (Cooldown <= 0)
        {
            money += placeBuilding.factoryCount * 10;
            power -= placeBuilding.factoryCount * 5;
            if (power < 0)
            {
                power = 0;
                money -= placeBuilding.factoryCount * 20;
            }

            Cooldown = 3f;
        }
    }
}
