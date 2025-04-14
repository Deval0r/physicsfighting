using UnityEngine;

public class RestaurantDoor : MonoBehaviour
{
    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpening = false;
    private bool isClosing = false;
    private float speed = 2f;

    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + new Vector3(0, 0, 3);
    }

    void Update()
    {
        if (isOpening)
        {
            transform.position = Vector3.Lerp(transform.position, openPosition, Time.deltaTime * speed);
        }
        else if (isClosing)
        {
            transform.position = Vector3.Lerp(transform.position, closedPosition, Time.deltaTime * speed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpening = true;
            isClosing = false;
            print("Door Opening");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpening = false;
            isClosing = true;
            print("Door Closing");
        }
    }
}
