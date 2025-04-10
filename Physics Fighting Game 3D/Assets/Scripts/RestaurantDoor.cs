using System.Numerics;
using UnityEngine;

public class RestaurantDoor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenDoor();
        }
    }
    private void OpenDoor()
    {
        // Logic to open the door
        transform.Rotate(0, 90, 0); // Example: Rotate the door 90 degrees around the Y-axis
    }
}
