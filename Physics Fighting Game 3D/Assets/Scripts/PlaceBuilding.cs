using UnityEngine;

public class PlaceBuilding : MonoBehaviour
{
    private Vector3 mousePosition;
    private Vector3 buildingPosition;
    public GameObject buildingPrefab;
    private GameObject buildingClone;
    private bool isSelectedBuilding;
    [SerializeField] private int scaleFactor;

    void Start()
    {
        buildingClone = null;
    }

    void Update()
    {
        if (isSelectedBuilding)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out RaycastHit hit))
            {
                buildingPosition = new Vector3(Mathf.Round(hit.point.x / scaleFactor) * scaleFactor, Mathf.Round(hit.point.y / scaleFactor) * scaleFactor, Mathf.Round(hit.point.z / scaleFactor) * scaleFactor);
                if (buildingClone != null)
                {
                    buildingClone.GetComponent<MeshRenderer>().material.color = Color.blue;
                    buildingClone.transform.position = buildingPosition;
                }
            }
            if (buildingClone == null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    buildingClone = Instantiate(buildingPrefab, buildingPosition, Quaternion.identity);
                    buildingClone.GetComponent<MeshRenderer>().material.color = Color.blue;
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Ray greenMouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(greenMouseRay, out RaycastHit hitGreen))
                {
                    buildingPosition = new Vector3(Mathf.Round(hitGreen.point.x / scaleFactor) * scaleFactor, Mathf.Round(hitGreen.point.y / scaleFactor) * scaleFactor, Mathf.Round(hitGreen.point.z / scaleFactor) * scaleFactor);
                    buildingClone.transform.position = buildingPosition;
                    Instantiate(buildingPrefab, buildingPosition, Quaternion.identity);
                    buildingClone.GetComponent<MeshRenderer>().material.color = Color.white;
                    buildingClone = null;
                    isSelectedBuilding = false;
                }
            }
        }
    }

    public void PlaceBuildings()
    {
        isSelectedBuilding = true;
    }
}
