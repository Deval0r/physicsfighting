using UnityEngine;

public class ToggleScript : MonoBehaviour
{
    [SerializeField] private GameObject TPObject;
    [SerializeField] private GameObject FPObject;

    public PlaceBuilding placeBuilding;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TPObject.SetActive(true);
        FPObject.SetActive(false);

        placeBuilding = FindObjectOfType<PlaceBuilding>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && TPObject.activeSelf)
        {
            TPObject.SetActive(false);
            FPObject.SetActive(true);

            placeBuilding.isSelectedBuilding = false;
            placeBuilding.isRemovingBuilding = false;

            Destroy(placeBuilding.buildingClonePrefab); 
            placeBuilding.buildingClonePrefab = null; 
        }
        else if (Input.GetKeyDown(KeyCode.P) && FPObject.activeSelf)
        {
            TPObject.SetActive(true);
            FPObject.SetActive(false);

            placeBuilding.isSelectedBuilding = false;
            placeBuilding.isRemovingBuilding = false;

            Destroy(placeBuilding.buildingClonePrefab); 
            placeBuilding.buildingClonePrefab = null; 
        }

        if (FPObject.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
