using UnityEngine;

public class MenuLight : MonoBehaviour
{
    private Camera mainCamera;
    private Light directionalLight;

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
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
        
        // Update light's position to match mouse
        directionalLight.transform.position = worldPos;
        
        // Reset rotation to zero
        transform.rotation = Quaternion.identity;
    }
}
