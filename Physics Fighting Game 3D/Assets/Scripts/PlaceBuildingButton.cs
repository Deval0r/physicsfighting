using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlaceBuildingButton : MonoBehaviour
{
    public bool isMouseOverUI;

    public PlaceBuilding placeBuilding;
    private Button placeButton;

    void Start()
    {
        placeBuilding = FindObjectOfType<PlaceBuilding>();
        placeButton = GetComponent<Button>();
    }

    void Update()
    {
        isMouseOverUI = EventSystem.current.IsPointerOverGameObject();

        ColorBlock colorBlock = placeButton.colors;
        if (placeBuilding.isSelectedBuilding && !placeButton.colors.selectedColor.Equals(Color.blue))
        {
            colorBlock.normalColor = Color.blue;
            colorBlock.highlightedColor = Color.blue;
            colorBlock.pressedColor = Color.blue;
            colorBlock.selectedColor = Color.blue;
            colorBlock.disabledColor = Color.blue;
        }
        else if (!placeBuilding.isSelectedBuilding && !placeButton.colors.selectedColor.Equals(Color.white))
        {
            colorBlock.normalColor = Color.white;
            colorBlock.highlightedColor = Color.white;
            colorBlock.pressedColor = Color.white;
            colorBlock.selectedColor = Color.white;
            colorBlock.disabledColor = Color.white;
        }
        placeButton.colors = colorBlock;
    }
}
