using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceBuildingButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isMouseOverPlaceBuldingButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        isMouseOverPlaceBuldingButton = true;
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        isMouseOverPlaceBuldingButton = false;
    }
}
