using UnityEngine;

public class ToggleScript : MonoBehaviour
{
    [SerializeField] private GameObject TPObject;
    [SerializeField] private GameObject FPObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TPObject.SetActive(true);
        FPObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && TPObject.activeSelf)
        {
            TPObject.SetActive(false);
            FPObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.P) && FPObject.activeSelf)
        {
            TPObject.SetActive(true);
            FPObject.SetActive(false);
        }

        if (FPObject.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
