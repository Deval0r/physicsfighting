using UnityEngine;

public class PlaceBuilding : MonoBehaviour
{
    private Vector3 mousePosition;
    private Vector3 buildingPosition;
    public GameObject buildingPrefab;
    private GameObject buildingClone;
    private bool isSelectedBuilding;
    private Material buildingMaterial;

    [SerializeField] private int scaleFactor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buildingMaterial = buildingPrefab.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        print(isSelectedBuilding);
        if (Input.GetMouseButtonDown(0) && isSelectedBuilding)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out RaycastHit hit))
            {
                buildingPosition = new Vector3(Mathf.Round(hit.point.x/scaleFactor) * scaleFactor, Mathf.Round(hit.point.y/scaleFactor) * scaleFactor, Mathf.Round(hit.point.z/scaleFactor)*scaleFactor );
                Instantiate(buildingPrefab, buildingPosition, Quaternion.identity);
                buildingClone.GetComponent<MeshRenderer>().material.color = Color.white;
            }
            isSelectedBuilding = false;
        }
        if (isSelectedBuilding && !Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out RaycastHit hit))
            {
                buildingPosition = new Vector3(Mathf.Round(hit.point.x/scaleFactor) * scaleFactor, Mathf.Round(hit.point.y/scaleFactor) * scaleFactor, Mathf.Round(hit.point.z/scaleFactor)*scaleFactor );
                buildingClone.GetComponent<MeshRenderer>().material.color = Color.green;
                buildingClone.transform.position = buildingPosition;
            }
            isSelectedBuilding = true;
        }
    }

    public void PlaceBuildings()
    {
        isSelectedBuilding = true;
    }
}