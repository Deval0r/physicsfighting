using UnityEngine;

public class SkyboxRotate : MonoBehaviour
{
    public float rotationSpeed = 1.0f; // Adjust this to control the speed of rotation

    void Update()
    {
        // Continuously rotate the skybox
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);
    }
}