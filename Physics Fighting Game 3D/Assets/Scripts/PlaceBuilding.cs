using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;


























//please dont edit this file too much 

public class PlaceBuilding : MonoBehaviour
{
    public GameManager gameManager;
    public PlaceBuildingButton placeBuildingButton;

    private Vector3 buildingPosition; 
    public int buildingIndex;
    private GameObject buildingClone; 
    public bool isSelectedBuilding; 
    public bool isRemovingBuilding;

    public bool isPlacedRestauant;

    public int factoryCount;
    public int batteryCount;
    public int bankCount;
    public int farmCount;
    public int windTurbineCount;

    private MeshRenderer removedBuildingMeshRenderer;
    public GameObject[] buildingPrefabs;
    public AudioSource soundSource;
    public AudioClip triggerSound;
    [SerializeField] private int scaleFactor;

    void Start() 
    {
        gameManager = FindObjectOfType<GameManager>();
        placeBuildingButton = FindObjectOfType<PlaceBuildingButton>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            buildingIndex = 0; 
            Destroy(buildingClone);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            buildingIndex = 1; 
            Destroy(buildingClone);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) 
        {
            buildingIndex = 2; 
            Destroy(buildingClone);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) 
        {
            buildingIndex = 3; 
            Destroy(buildingClone);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) 
        {
            buildingIndex = 4; 
            Destroy(buildingClone);
        }
        if (Input.GetMouseButtonDown(0) && placeBuildingButton.isMouseOverUI && isSelectedBuilding) 
        {
            isSelectedBuilding = false; 
            Destroy(buildingClone); 
            buildingClone = null; 
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !placeBuildingButton.isMouseOverUI) 
        {
            isSelectedBuilding = false; 
            isRemovingBuilding = false;
            Destroy(buildingClone); 
            buildingClone = null; 
        }
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

            if (Input.GetMouseButtonDown(0) && gameManager.money >= 500  && !placeBuildingButton.isMouseOverUI && Physics.Raycast(buildingClone.transform.position, Vector3.down, out RaycastHit hitInfo, 1)) 
            {
                GameObject placedBuilding = Instantiate(buildingPrefabs[buildingIndex], buildingClone.transform.position, buildingClone.transform.rotation); 
                soundSource.PlayOneShot(triggerSound);
                placedBuilding.GetComponent<Collider>().enabled = true;
                Destroy(buildingClone); 
                buildingClone = null; 
                if (buildingIndex == 0)
                {
                    factoryCount++;
                }
                else if (buildingIndex == 1)
                {
                    batteryCount++;
                }
                else if (buildingIndex == 2)
                {
                    bankCount++;
                }
                else if (buildingIndex == 3)
                {
                    farmCount++;
                    isPlacedRestauant = true;
                }
                else if (buildingIndex == 4)
                {
                    windTurbineCount++;
                }
                gameManager.money -= 500;
                if (gameManager.money >= 500)
                {
                    buildingClone = Instantiate(buildingPrefabs[buildingIndex]); 
                    buildingClone.GetComponent<MeshRenderer>().material.color = Color.blue; 
                    buildingClone.GetComponent<Collider>().enabled = false; 
                } else 
                {
                    isSelectedBuilding = false;
                }
            }
            if (Input.GetMouseButtonDown(0) && gameManager.money < 500  && !placeBuildingButton.isMouseOverUI) 
            {
                isSelectedBuilding = false;
                buildingClone.GetComponent<MeshRenderer>().material.color = Color.yellow;
                Destroy(buildingClone, 0.1f);
                buildingClone = null;
            }
        }
        if (isRemovingBuilding && !isSelectedBuilding  && !placeBuildingButton.isMouseOverUI &&  Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo2))
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
                    if (buildingIndex == 0)
                    {
                        factoryCount--;
                    }
                    else if (buildingIndex == 1)
                    {
                        batteryCount--;
                    }
                    else if (buildingIndex == 2)
                    {
                        bankCount--;
                    } 
                    else if (buildingIndex == 3)
                    {
                        farmCount--;
                    }
                    else if (buildingIndex == 4)
                    {
                        windTurbineCount--;
                    }
                } 
            } else if (removedBuildingMeshRenderer != null)
            {
                removedBuildingMeshRenderer.material.color = Color.white;
            }
        }
    }

    public void PlaceBuildings() 
    { 
        if (isRemovingBuilding) 
        { 
            isRemovingBuilding = false; 
            Destroy(buildingClone); 
            buildingClone = null; 
        }
        isSelectedBuilding = !isSelectedBuilding;
        if (isSelectedBuilding) 
        {
            if (buildingClone == null) 
            { 
                buildingClone = Instantiate(buildingPrefabs[buildingIndex]); 
                buildingClone.GetComponent<MeshRenderer>().material.color = Color.blue; 
                buildingClone.GetComponent<Collider>().enabled = false; 
                isSelectedBuilding = true; 
            }
            else
            {
                Destroy(buildingClone); 
                buildingClone = null;
            }
        }
    }

    public void RemoveBuildings() 
    { 
        isRemovingBuilding = !isRemovingBuilding;
    }
}