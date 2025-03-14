using UnityEngine;

public class PlaceBuilding : MonoBehaviour
{
    private Vector3 mousePosition;
    private Vector3 buildingPosition;
    public GameObject buildingPrefab;
    private GameObject buildingClone;
    private bool isSelectedBuilding;
    [SerializeField] private int scaleFactor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buildingClone = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelectedBuilding)
        {
            // Raycast from the mouse position to the world
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out RaycastHit hit))
            {
                // Calculate the snapped position based on scaleFactor
                buildingPosition = new Vector3(Mathf.Round(hit.point.x / scaleFactor) * scaleFactor,
                                               Mathf.Round(hit.point.y / scaleFactor) * scaleFactor,
                                               Mathf.Round(hit.point.z / scaleFactor) * scaleFactor);

                // Move the building clone to the new position and change color
                if (buildingClone != null)
                {
                    buildingClone.GetComponent<MeshRenderer>().material.color = Color.green;
                    buildingClone.transform.position = buildingPosition;
                }
            }

            // If the buildingClone is not instantiated, instantiate it when the mouse is clicked
            if (buildingClone == null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // Instantiate the building prefab at the snapped position
                    buildingClone = Instantiate(buildingPrefab, buildingPosition, Quaternion.identity);
                    buildingClone.GetComponent<MeshRenderer>().material.color = Color.green;
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                // Confirm the placement of the building
                Ray greenMouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(greenMouseRay, out RaycastHit hitGreen))
                {
                    // Snapping the position again after the click
                    buildingPosition = new Vector3(Mathf.Round(hitGreen.point.x / scaleFactor) * scaleFactor,
                                                   Mathf.Round(hitGreen.point.y / scaleFactor) * scaleFactor,
                                                   Mathf.Round(hitGreen.point.z / scaleFactor) * scaleFactor);
                    buildingClone.transform.position = buildingPosition;

                    // Instantiate the building prefab at the snapped position
                    Instantiate(buildingPrefab, buildingPosition, Quaternion.identity);

                    // Reset the color of the building clone to white after placement
                    buildingClone.GetComponent<MeshRenderer>().material.color = Color.white;
                    buildingClone = null; // Reset clone
                    isSelectedBuilding = false; // Deselect the building
                }
            }
        }
    }

    // This method is called to start building placement
    public void PlaceBuildings()
    {
        isSelectedBuilding = true;
    }
}
