using UnityEngine;

public class DisableFogForNuke : MonoBehaviour
{
    void OnPreRender()
    {
        RenderSettings.fog = false; // Disable fog before rendering the nuke
    }

    void OnPostRender()
    {
        RenderSettings.fog = true; // Re-enable fog after the nuke is rendered
    }
}
