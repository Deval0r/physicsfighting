using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;

//please dont edit this file

public class PlaceBuilding : MonoBehaviour
{
    public GameManager gameManager;
    public PlaceBuildingButton placeBuildingButton;

    private Vector3 buildingPosition; 
    public int buildingIndex;
    private GameObject buildingClone; 
    public bool isSelectedBuilding; 
    public bool isRemovingBuilding;
    public int factoryCount;
    public int batteryCount;
    public int bankCount;
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
        print(isSelectedBuilding);
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            buildingIndex = 0; 
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            buildingIndex = 1; 
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) 
        {
            buildingIndex = 2; 
        }
        //destroy buiding clone if mouse over ui
        if(placeBuildingButton.isMouseOverUI && isSelectedBuilding) 
        {
            isSelectedBuilding = false; 
            Destroy(buildingClone); 
            buildingClone = null; 
        }
        if (isSelectedBuilding && buildingClone != null && !placeBuildingButton.isMouseOverUI) 
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
                } 
            } else if (removedBuildingMeshRenderer != null)
            {
                removedBuildingMeshRenderer.material.color = Color.white;
            }
        }
    }

    public void PlaceBuildings() 
    { 
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

    public void CancelBuilding() 
    { 
        /*if (isSelectedBuilding) 
        { 
            isSelectedBuilding = false; 
            Destroy(buildingClone); 
            buildingClone = null; 
        }*/
    }
}