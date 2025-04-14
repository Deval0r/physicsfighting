using UnityEngine;

public class ObjectEnableDisable : MonoBehaviour
{
    public GameObject targetObject; // The object to disable and later enable
    public GameObject alternateObject; // The object to disable when the target is enabled
    public bool enableCondition = false; // This condition will be checked repeatedly

    void Start()
    {
        // Start the coroutine to handle the enable-disable sequence
        StartCoroutine(HandleObjectEnableDisable());
    }

    IEnumerator HandleObjectEnableDisable()
    {
        // Disable the target object initially
        if (targetObject != null)
        {
            targetObject.SetActive(false);
            Debug.Log("Target object disabled.");
        }

        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Repeatedly check the condition to enable the target object
        while (true)
        {
            if (enableCondition) // Condition to enable the target object
            {
                if (targetObject != null)
                {
                    targetObject.SetActive(true);
                    Debug.Log("Target object enabled.");
                }

                if (alternateObject != null)
                {
                    alternateObject.SetActive(false);
                    Debug.Log("Alternate object disabled.");
                }

                // Exit the loop since the action is completed
                yield break;
            }

            // Wait for a short interval before checking the condition again
            yield return new WaitForSeconds(0.1f);
        }
    }
}
