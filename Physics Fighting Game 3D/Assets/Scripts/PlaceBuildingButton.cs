using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceBuildingButton : MonoBehaviour
{
    public bool isMouseOverUI;

    void Update()
    {
        isMouseOverUI = EventSystem.current.IsPointerOverGameObject();
    }
}
