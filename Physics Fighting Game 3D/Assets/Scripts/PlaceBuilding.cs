using UnityEngine;

public class PlaceBuilding : MonoBehaviour
{
    private Vector3 buildingPosition; 
    public GameObject buildingPrefab; 
    private int buildingIndex;
    public GameManager gameManager;
    private GameObject buildingClone; 
    private bool isSelectedBuilding; 
    private bool isRemovingBuilding;
    public int buildingCount;

    public AudioSource soundSource; // AudioSource to play the sound
    public AudioClip triggerSound;
    [SerializeField] private int scaleFactor;

    void Start() 
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void Update()
    {
        print(isRemovingBuilding);
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
                Destroy(buildingClone); 
                buildingClone = null; 
                isSelectedBuilding = false;
            }
            if (isRemovingBuilding && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo2))
            {
                    // Check if the object hit by the raycast is tagged as "Building"
                if (hitInfo2.collider.gameObject.tag == "Building") 
                {
                    // Change the color of the building to red (for visual feedback)
                    MeshRenderer meshRenderer = hitInfo2.collider.gameObject.GetComponent<MeshRenderer>();
                    if (meshRenderer != null)
                    {
                        meshRenderer.material.color = Color.red;
                    }

                    // Destroy the building and decrement the building count
                    //Destroy(hitInfo2.collider.gameObject); 
                    //buildingCount--;
                }
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
