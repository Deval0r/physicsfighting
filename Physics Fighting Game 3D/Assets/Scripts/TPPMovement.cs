using UnityEngine;

public class TPPMovement : MonoBehaviour
{
    private float speed;
    public ThirdPersonCameraView2 cameraView;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = 10.0f;
        cameraView = FindObjectOfType<ThirdPersonCameraView2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * Time.deltaTime * speed;
        }
    }
}
