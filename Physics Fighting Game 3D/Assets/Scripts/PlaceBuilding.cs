using NUnit.Framework;
using UnityEngine;

public class PlaceBuilding : MonoBehaviour
{
    private Vector3 buildingPosition; 
    public GameObject buildingPrefab; 
    private int buildingIndex;
    public GameManager gameManager;
    private GameObject buildingClone; 
    public bool isSelectedBuilding; 
    public bool isRemovingBuilding;
    public int buildingCount;
    private MeshRenderer removedBuildingMeshRenderer;

    public AudioSource soundSource; // AudioSource to play the sound
    public AudioClip triggerSound;
    [SerializeField] private int scaleFactor;

    void Start() 
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void Update()
    {
        print(isSelectedBuilding);
        if (isSelectedBuilding && buildingClone != null) 
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                buildingClone.transform.position = new Vector3(Mathf.Round(hit.point.x / scaleFactor) * scaleFactor, Mathf.Round(hit.point.y / 3.3f) * 3.3f, Mathf.Round(hit.point.z / scaleFactor) * scaleFactor);
                if (Input.GetKeyDown(KeyCode.E)) 
                {
                    buildingClone.transform.Rotate(0, 90, 0);
                }
                if (Input.GetKeyDown(KeyCode.Q)) 
                {
                    buildingClone.transform.Rotate(0, -90, 0);
                }
            }

            if (Input.GetMouseButtonDown(0) && gameManager.money >= 500 && Physics.Raycast(buildingClone.transform.position, Vector3.down, out RaycastHit hitInfo, 1)) 
            {
                GameObject placedBuilding = Instantiate(buildingPrefab, buildingClone.transform.position, buildingClone.transform.rotation); 
                soundSource.PlayOneShot(triggerSound);
                placedBuilding.GetComponent<Collider>().enabled = true;
                Destroy(buildingClone); 
                buildingClone = null; 
                isSelectedBuilding = false;
                buildingCount++;
                gameManager.money -= 500;
            }
            if (Input.GetMouseButtonDown(0) && gameManager.money < 500) 
            {
                buildingClone.GetComponent<MeshRenderer>().material.color = Color.yellow;
                Destroy(buildingClone, 0.1f);
                buildingClone = null; 
            }
        }
        if (isRemovingBuilding && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo2))
        {
            if (hitInfo2.collider.gameObject.tag == "Building")
            {
                if (removedBuildingMeshRenderer != null && removedBuildingMeshRenderer != hitInfo2.collider.gameObject.GetComponent<MeshRenderer>())
                {
                    removedBuildingMeshRenderer.material.color = Color.white;
                }
                removedBuildingMeshRenderer = hitInfo2.collider.gameObject.GetComponent<MeshRenderer>();
                removedBuildingMeshRenderer.material.color = Color.red;
                if (Input.GetMouseButtonDown(0) && !isSelectedBuilding)
                {
                    gameManager.money += 250;
                    Destroy(hitInfo2.collider.gameObject);
                    buildingCount--;
                } 
            } else if (removedBuildingMeshRenderer != null)
            {
                removedBuildingMeshRenderer.material.color = Color.white;
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

    public void RemoveBuildings() 
    { 
        isRemovingBuilding = !isRemovingBuilding;
    }
}
