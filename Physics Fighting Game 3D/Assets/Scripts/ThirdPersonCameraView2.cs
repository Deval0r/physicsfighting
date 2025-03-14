using UnityEngine;

public class ThirdPersonCameraView2 : MonoBehaviour
{

    private float rotationX;
    private float rotationY;

    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float moveSpeed;

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            //all this means is that when pressing right mouse button in unity the cursor will not be visible
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            rotationX += mouseX;
            rotationY -= mouseY; //this line ensures that rotationY will not be inverted due to weird unity stuff
            rotationY = Mathf.Clamp(rotationY, -90f, 90f);
            transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);
        } else 
        {
            //again, this is for making the cursor visible when not pressing right mouse button
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * Time.deltaTime * moveSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * Time.deltaTime * moveSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * Time.deltaTime * moveSpeed;
        }
    }
}
