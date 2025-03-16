using UnityEngine;

public class PlaceBuilding : MonoBehaviour
{
    private Vector3 buildingPosition; 
    public GameObject buildingPrefab; 
    private GameObject buildingClone; 
    private bool isSelectedBuilding; 
    [SerializeField] private int scaleFactor;

    void Update()
    {
        if (isSelectedBuilding && buildingClone != null) 
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit)) 
                buildingClone.transform.position = new Vector3(Mathf.Round(hit.point.x / scaleFactor) * scaleFactor, Mathf.Round(hit.point.y / scaleFactor) * scaleFactor, Mathf.Round(hit.point.z / scaleFactor) * scaleFactor);

            if (Input.GetMouseButtonDown(0)) 
            {
                Instantiate(buildingPrefab, buildingClone.transform.position, Quaternion.identity); 
                Destroy(buildingClone); 
                buildingClone = null; 
                isSelectedBuilding = false;
            }
        }
    }

    public void PlaceBuildings() 
    { 
        if (buildingClone == null) 
        { 
            buildingClone = Instantiate(buildingPrefab); 
            buildingClone.GetComponent<MeshRenderer>().material.color = Color.blue; 
            isSelectedBuilding = true; 
        } 
    }
}
