using UnityEngine;
using UnityEngine.UI;

public class RemoveButtonScript : MonoBehaviour
{
    public PlaceBuilding placeBuilding;
    private Button removeButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        placeBuilding = FindObjectOfType<PlaceBuilding>();
        removeButton = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (placeBuilding.isRemovingBuilding && !removeButton.colors.selectedColor.Equals(Color.red))
        {
            ColorBlock colorBlock = removeButton.colors;
            colorBlock.normalColor = Color.red;
            colorBlock.highlightedColor = Color.red;
            colorBlock.pressedColor = Color.red;
            colorBlock.selectedColor = Color.red;
            colorBlock.disabledColor = Color.red;
            removeButton.colors = colorBlock;
        } else if(!placeBuilding.isRemovingBuilding && !removeButton.colors.selectedColor.Equals(Color.white))
        {
            ColorBlock colorBlock = removeButton.colors;
            colorBlock.normalColor = Color.white;
            colorBlock.highlightedColor = Color.white;
            colorBlock.pressedColor = Color.white;
            colorBlock.selectedColor = Color.white;
            colorBlock.disabledColor = Color.white;
            removeButton.colors = colorBlock;
        }
    }
}
