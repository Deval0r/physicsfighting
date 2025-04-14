using UnityEngine;

public class FoodSpawn : MonoBehaviour
{
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private GameObject foodSpawnPoint;
    
    private float coolDown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coolDown = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (coolDown <= 0)
        {
            SpawnFood();
            coolDown = 3;
        }
        else
        {
            coolDown -= Time.deltaTime;
        }
    }
    private void SpawnFood()
    {
        Vector3 spawnPosition = new Vector3(foodSpawnPoint.transform.position.x + Random.Range(-3.5f, 3), foodSpawnPoint.transform.position.y, foodSpawnPoint.transform.position.z + Random.Range(-7, 7));
        Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
    }
}
