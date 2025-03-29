using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems; // Add this for UI interaction checks

public class PlaceBuilding : MonoBehaviour
{
    public BuildingData[] buildings;  // Replace buildingPrefabs and buildingCosts arrays
    private Vector3 buildingPosition;
    public Image[] hotbarSlots;
    private int selectedBuildingIndex;
    public GameManager gameManager;
    private GameObject buildingClone;
    public bool isSelectedBuilding;
    public bool isRemovingBuilding;
    public int buildingCount;
    private MeshRenderer removedBuildingMeshRenderer;

    public AudioSource soundSource;
    public AudioClip triggerSound;
    [SerializeField] private int scaleFactor;

    public TextMeshProUGUI placeButtonText;    // Reference to place button text
    public TextMeshProUGUI removeButtonText;   // Reference to remove button text

    public int bankCount; // Add this to track number of banks specifically
    public int batteryCount; // Add this to track number of batteries

    public Image placeButtonImage;    // Reference to place button image
    public Image removeButtonImage;   // Reference to remove button image

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        UpdateHotbarHighlight(); // Initialize hotbar highlighting
        UpdateButtonTexts(); // Initialize button texts
    }

    void Update()
    {
        HandleHotbarInput();
        
        // Only show preview if we have a valid building selected
        if (isSelectedBuilding && buildingClone != null)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                buildingClone.transform.position = new Vector3(
                    Mathf.Round(hit.point.x / scaleFactor) * scaleFactor,
                    Mathf.Round(hit.point.y / 3.3f) * 3.3f,
                    Mathf.Round(hit.point.z / scaleFactor) * scaleFactor
                );

                if (Input.GetKeyDown(KeyCode.E))
                {
                    buildingClone.transform.Rotate(0, 90, 0);
                }

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    buildingClone.transform.Rotate(0, -90, 0);
                }
            }

            // Add UI check before processing building placement
            if (Input.GetMouseButtonDown(0) && !IsPointerOverUI() &&
                gameManager.money >= buildings[selectedBuildingIndex].placeCost &&
                Physics.Raycast(buildingClone.transform.position, Vector3.down, out RaycastHit hitInfo, 1))
            {
                GameObject placedBuilding = Instantiate(
                    buildings[selectedBuildingIndex].buildingPrefab,
                    buildingClone.transform.position,
                    buildingClone.transform.rotation
                );
                
                // Update counts based on building type
                if (buildings[selectedBuildingIndex].buildingName.ToLower() == "bank")
                {
                    bankCount++;
                    gameManager.UpdateMaxMoney(bankCount * 1500);
                }
                else if (buildings[selectedBuildingIndex].buildingName.ToLower() == "battery")
                {
                    batteryCount++;
                    Debug.Log($"Battery placed. New count: {batteryCount}, New max power: {batteryCount * 1000}");
                    gameManager.UpdateMaxPower(batteryCount * 1000);
                }

                soundSource.PlayOneShot(triggerSound);
                placedBuilding.GetComponent<Collider>().enabled = true;
<<<<<<< HEAD
<<<<<<< HEAD
                Destroy(buildingClone);
                buildingClone = null;
                buildingCount++;
                gameManager.money -= buildings[selectedBuildingIndex].placeCost;
                if (gameManager.money >= buildings[selectedBuildingIndex].placeCost)
                {
                    buildingClone = Instantiate(buildings[selectedBuildingIndex].buildingPrefab);
                    buildingClone.GetComponent<MeshRenderer>().material.color = Color.blue;
                    buildingClone.GetComponent<Collider>().enabled = false;
                }
                else
                {
                    isSelectedBuilding = false;
                }
=======
                Destroy(buildingClone); 
                buildingClone = null; 
                isSelectedBuilding = false;
                buildingCount++;
                gameManager.money -= 500;
>>>>>>> parent of 5a60f35 (Stay selected buildings)
=======
                Destroy(buildingClone); 
                buildingClone = null; 
                isSelectedBuilding = false;
                buildingCount++;
                gameManager.money -= 500;
>>>>>>> parent of 5a60f35 (Stay selected buildings)
            }
            else if (Input.GetMouseButtonDown(0) && !IsPointerOverUI() && gameManager.money < buildings[selectedBuildingIndex].placeCost)
            {
                buildingClone.GetComponent<MeshRenderer>().material.color = Color.yellow;
                Destroy(buildingClone, 0.1f);
                buildingClone = null;
            }
        }
        else if (buildingClone != null)
        {
            // Clean up any orphaned preview
            Destroy(buildingClone);
            buildingClone = null;
        }

        if (isRemovingBuilding && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo2))
        {
            if (hitInfo2.collider.gameObject.CompareTag("Building"))
            {
                if (removedBuildingMeshRenderer != null && removedBuildingMeshRenderer != hitInfo2.collider.gameObject.GetComponent<MeshRenderer>())
                {
                    removedBuildingMeshRenderer.material.color = Color.white;
                }
                removedBuildingMeshRenderer = hitInfo2.collider.gameObject.GetComponent<MeshRenderer>();
                removedBuildingMeshRenderer.material.color = Color.red;
                UpdateButtonTexts();
                
                // Add UI check before processing building removal
                if (Input.GetMouseButtonDown(0) && !IsPointerOverUI() && !isSelectedBuilding)
                {
                    foreach (BuildingData building in buildings)
                    {
                        if (hitInfo2.collider.gameObject.name.ToLower().Contains(building.buildingName.ToLower()))
                        {
                            if (building.buildingName.ToLower() == "bank")
                            {
                                bankCount--;
                                gameManager.UpdateMaxMoney(bankCount * 1500);
                            }
                            else if (building.buildingName.ToLower() == "battery")
                            {
                                batteryCount--;
                                gameManager.UpdateMaxPower(batteryCount * 1000);
                            }
                            gameManager.money += building.removeCost;
                            break;
                        }
                    }
                    Destroy(hitInfo2.collider.gameObject);
                    buildingCount--;
                }
            }
            else if (removedBuildingMeshRenderer != null)
            {
                removedBuildingMeshRenderer.material.color = Color.white;
                UpdateButtonTexts();
            }
        }
    }

<<<<<<< HEAD
<<<<<<< HEAD
    void HandleHotbarInput()
    {
        // Check for keys 1-5 to select a building
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectBuilding(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectBuilding(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectBuilding(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectBuilding(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectBuilding(4);
    }

    void SelectBuilding(int index)
    {
        if (index >= 0 && index < buildings.Length)
        {
            // Destroy existing preview if it exists
            if (buildingClone != null)
            {
                Destroy(buildingClone);
                buildingClone = null;
            }

            selectedBuildingIndex = index;
            UpdateHotbarHighlight();
            
            // Create new preview
            buildingClone = Instantiate(buildings[selectedBuildingIndex].buildingPrefab);
            buildingClone.GetComponent<MeshRenderer>().material.color = Color.blue;
            buildingClone.GetComponent<Collider>().enabled = false;
            
            isSelectedBuilding = true;
            UpdateButtonTexts(); // Update texts when selecting new building
            Debug.Log($"Selected building: {buildings[selectedBuildingIndex].buildingName}, Cost: {buildings[selectedBuildingIndex].placeCost}");
        }
    }

    void UpdateHotbarHighlight()
    {
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            if (i == selectedBuildingIndex)
            {
                hotbarSlots[i].color = Color.green; // Highlight selected slot
            }
            else
            {
                hotbarSlots[i].color = Color.white; // Reset other slots
            }
        }
    }

    void UpdateButtonTexts()
    {
        if (placeButtonText != null)
        {
            placeButtonText.text = $"Place: -{buildings[selectedBuildingIndex].placeCost}";
        }

        if (removeButtonText != null)
        {
            if (isRemovingBuilding && removedBuildingMeshRenderer != null)
            {
                foreach (BuildingData building in buildings)
                {
                    if (removedBuildingMeshRenderer.gameObject.name.ToLower().Contains(building.buildingName.ToLower()))
                    {
                        removeButtonText.text = $"Remove: +{building.removeCost}";
                        Debug.Log($"Highlighting {building.buildingName} for removal");
                        break;
                    }
                }
            }
            else
            {
                removeButtonText.text = "Remove: +0";
            }
        }

        // Update button colors
        if (placeButtonImage != null)
        {
            placeButtonImage.color = isSelectedBuilding ? Color.blue : Color.white;
        }
        if (removeButtonImage != null)
        {
            removeButtonImage.color = isRemovingBuilding ? Color.red : Color.white;
        }
    }

    public void PlaceBuildings()
    {
        // If already in build mode, turn it off
        if (isSelectedBuilding)
        {
            isSelectedBuilding = false;
            if (buildingClone != null)
            {
                Destroy(buildingClone);
                buildingClone = null;
            }
        }
        else
        {
            // Switch to build mode
            isRemovingBuilding = false;
            isSelectedBuilding = true;
            
            // Create new preview
            buildingClone = Instantiate(buildings[selectedBuildingIndex].buildingPrefab);
            buildingClone.GetComponent<MeshRenderer>().material.color = Color.blue;
            buildingClone.GetComponent<Collider>().enabled = false;
        }
        
        UpdateButtonTexts();
=======
=======
>>>>>>> parent of 5a60f35 (Stay selected buildings)
    public void PlaceBuildings() 
    { 
        if (buildingClone == null) 
        { 
            buildingClone = Instantiate(buildingPrefab); 
            buildingClone.GetComponent<MeshRenderer>().material.color = Color.blue; 
            buildingClone.GetComponent<Collider>().enabled = false; 
            isSelectedBuilding = true; 
        } 
<<<<<<< HEAD
>>>>>>> parent of 5a60f35 (Stay selected buildings)
=======
>>>>>>> parent of 5a60f35 (Stay selected buildings)
    }

    public void RemoveBuildings()
    {
        // Toggle remove mode
        isRemovingBuilding = !isRemovingBuilding;
        
        // If switching to remove mode, ensure build mode is off
        if (isRemovingBuilding)
        {
            isSelectedBuilding = false;
            if (buildingClone != null)
            {
                Destroy(buildingClone);
                buildingClone = null;
            }
        }
        
        // Reset any highlighted building color when toggling off
        if (!isRemovingBuilding && removedBuildingMeshRenderer != null)
        {
            removedBuildingMeshRenderer.material.color = Color.white;
            removedBuildingMeshRenderer = null;
        }
        
        UpdateButtonTexts();
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
