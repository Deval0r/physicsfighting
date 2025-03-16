using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab; // Prefab of the zombie to spawn
    public float spawnRadius = 10f; // Radius around the spawner to spawn zombies
    public float spawnInterval = 3f; // Time between spawns in seconds

    private bool isSpawning = true; // Controls whether spawning is active

    void Start()
    {
        // Start the spawning process
        StartCoroutine(SpawnZombies());
    }

    System.Collections.IEnumerator SpawnZombies()
    {
        while (isSpawning)
        {
            // Wait for the specified interval
            yield return new WaitForSeconds(spawnInterval);

            // Generate a random position within the radius
            Vector3 spawnPosition = GenerateRandomPosition();

            // Instantiate a zombie at the random position
            Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
            Debug.Log($"Zombie spawned at {spawnPosition}");
        }
    }

    Vector3 GenerateRandomPosition()
    {
        // Get a random point within a circle on the XZ plane
        Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;

        // Convert to a 3D point, keeping the Y-coordinate the same as the spawner
        Vector3 spawnPosition = new Vector3(
            transform.position.x + randomPoint.x,
            transform.position.y,
            transform.position.z + randomPoint.y
        );

        return spawnPosition;
    }

    // Optional: Call this method to stop spawning
    public void StopSpawning()
    {
        isSpawning = false;
    }
}
