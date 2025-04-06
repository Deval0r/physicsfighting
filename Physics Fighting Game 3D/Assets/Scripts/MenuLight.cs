using UnityEngine;

public class MenuLight : MonoBehaviour
{
    private Camera mainCamera;
    private Light directionalLight;
    public float height = 10f; // Height of the light above the scene
    public float smoothSpeed = 5f; // Speed at which the light moves to follow the mouse

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        directionalLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get mouse position in screen coordinates
        Vector3 mousePos = Input.mousePosition;
        
        // Convert screen position to world position
        Vector3 targetPos = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, height));
        
        // Smoothly move the light towards the target position
        Vector3 newPosition = Vector3.Lerp(transform.position, new Vector3(targetPos.x, height, targetPos.z), Time.deltaTime * smoothSpeed);
        transform.position = newPosition;
        
        // Keep the light pointing downward
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }
}
