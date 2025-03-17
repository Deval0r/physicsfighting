using UnityEngine;

public class ToggleScript : MonoBehaviour
{
    [SerializeField] private GameObject TPCamera;
    [SerializeField] private GameObject FPCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && TPCamera.activeSelf)
        {
            TPCamera.SetActive(false);
            FPCamera.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.P) && FPCamera.activeSelf)
        {
            TPCamera.SetActive(true);
            FPCamera.SetActive(false);
        }
    }
}
