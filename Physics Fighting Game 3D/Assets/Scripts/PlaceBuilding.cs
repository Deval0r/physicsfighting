using UnityEngine;

public class PlaceBuilding : MonoBehaviour
{
    private Vector3 mousePosition;
    private Vector3 buildingPosition;
    public GameObject buildingPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PlaceBuildings();
    }

    private void PlaceBuildings()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out RaycastHit hit))
            {
                buildingPosition = new Vector3(Mathf.Round(hit.point.x), Mathf.Round(hit.point.y), Mathf.Round(hit.point.z));
                Instantiate(buildingPrefab, buildingPosition, Quaternion.identity);
            }
        }
    }
}