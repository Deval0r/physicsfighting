using UnityEngine;
using UnityEngine.UI;

public class PlaceBuilding : MonoBehaviour
{
    private Vector3 buildingPosition;
    public GameObject[] buildingPrefabs;  // Array of building prefabs
    public int[] buildingCosts;          // Array of building costs (set in the Inspector)
    public Image[] hotbarSlots;          // Array of images representing hotbar slots
    private int selectedBuildingIndex;   // Current index of the selected building
    public GameManager gameManager;      // Reference to GameManager
    private GameObject buildingClone;
    public bool isSelectedBuilding;
    public bool isRemovingBuilding;
    public int buildingCount;
    private MeshRenderer removedBuildingMeshRenderer;

    public AudioSource soundSource;      // AudioSource to play the sound
    public AudioClip triggerSound;
    [SerializeField] private int scaleFactor;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        UpdateHotbarHighlight(); // Initialize hotbar highlighting
    }

    void Update()
    {
        HandleHotbarInput(); // Check for hotbar key input
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

            if (Input.GetMouseButtonDown(0) &&
                gameManager.money >= buildingCosts[selectedBuildingIndex] &&
                Physics.Raycast(buildingClone.transform.position, Vector3.down, out RaycastHit hitInfo, 1))
            {
                GameObject placedBuilding = Instantiate(
                    buildingPrefabs[selectedBuildingIndex],
                    buildingClone.transform.position,
                    buildingClone.transform.rotation
                );
                soundSource.PlayOneShot(triggerSound);
                placedBuilding.GetComponent<Collider>().enabled = true;
                Destroy(buildingClone);
                buildingClone = null;
                buildingCount++;
                gameManager.money -= buildingCosts[selectedBuildingIndex];
                if (gameManager.money >= buildingCosts[selectedBuildingIndex])
                {
                    buildingClone = Instantiate(buildingPrefabs[selectedBuildingIndex]);
                    buildingClone.GetComponent<MeshRenderer>().material.color = Color.blue;
                    buildingClone.GetComponent<Collider>().enabled = false;
                }
                else
                {
                    isSelectedBuilding = false;
                }
            }
            else if (Input.GetMouseButtonDown(0) && gameManager.money < buildingCosts[selectedBuildingIndex])
            {
                buildingClone.GetComponent<MeshRenderer>().material.color = Color.yellow;
                Destroy(buildingClone, 0.1f);
                buildingClone = null;
            }
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
                if (Input.GetMouseButtonDown(0) && !isSelectedBuilding)
                {
                    gameManager.money += 250; // Refund half the cost
                    Destroy(hitInfo2.collider.gameObject);
                    buildingCount--;
                }
            }
            else if (removedBuildingMeshRenderer != null)
            {
                removedBuildingMeshRenderer.material.color = Color.white;
            }
        }
    }

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
        if (index >= 0 && index < buildingPrefabs.Length)
        {
            selectedBuildingIndex = index; // Update selected index
            UpdateHotbarHighlight(); // Update the hotbar UI
            PlaceBuildings(); // Begin placing the new building
            Debug.Log($"Selected building index: {selectedBuildingIndex}, Cost: {buildingCosts[selectedBuildingIndex]}");
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

    public void PlaceBuildings()
    {
        isSelectedBuilding = true;
        if (buildingClone == null)
        {
            buildingClone = Instantiate(buildingPrefabs[selectedBuildingIndex]);
            buildingClone.GetComponent<MeshRenderer>().material.color = Color.blue;
            buildingClone.GetComponent<Collider>().enabled = false;
        }
        else
        {
            Destroy(buildingClone);
            buildingClone = null;
        }
    }

    public void RemoveBuildings()
    {
        isRemovingBuilding = !isRemovingBuilding;
    }
}
