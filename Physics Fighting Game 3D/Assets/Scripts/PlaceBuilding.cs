using UnityEngine;

public class PlaceBuilding : MonoBehaviour
{
    private Vector3 buildingPosition; 
    public GameObject buildingPrefab; 
    public GameManager gameManager;
    private GameObject buildingClone; 
    private bool isSelectedBuilding; 
    [SerializeField] private int scaleFactor;

    void Start() 
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void Update()
    {
        if (isSelectedBuilding && buildingClone != null) 
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                buildingClone.transform.position = new Vector3(Mathf.Round(hit.point.x / scaleFactor) * scaleFactor, Mathf.Round(hit.point.y / 3.3f) * 3.3f, Mathf.Round(hit.point.z / scaleFactor) * scaleFactor);

            if (Input.GetMouseButtonDown(0) && gameManager.money >= 500) 
            {
                GameObject placedBuilding = Instantiate(buildingPrefab, buildingClone.transform.position, Quaternion.identity); 
                placedBuilding.GetComponent<Collider>().enabled = true;
                Destroy(buildingClone); 
                buildingClone = null; 
                isSelectedBuilding = false;
                gameManager.money -= 500;
            }
        }
    }

    public void PlaceBuildings() 
    { 
        if (buildingClone == null) 
        { 
            buildingClone = Instantiate(buildingPrefab); 
            buildingClone.GetComponent<MeshRenderer>().material.color = Color.blue; 
            buildingClone.GetComponent<Collider>().enabled = false; 
            isSelectedBuilding = true; 
        } 
    }
}
