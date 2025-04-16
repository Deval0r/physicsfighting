using UnityEngine;
using System.Collections;

public class ObjectEnableDisable : MonoBehaviour
{
    public GameObject targetObject; // The object to enable
    public GameObject alternateObject; // The object to disable when the target is enabled
    public GameObject observedObject; // The object to monitor for activation
    public AudioSource audioSource; // The audio source to fade out and disable
    public bool enableCondition = false; // Condition will be updated dynamically
    public float fadeDuration = 1f; // Time in seconds for audio fade-out

    void Start()
    {
        StartCoroutine(HandleObjectEnableDisable());
    }

    IEnumerator HandleObjectEnableDisable()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
            Debug.Log("Target object disabled.");
        }

        yield return new WaitForSeconds(1f);

        while (true)
        {
            if (observedObject != null && observedObject.activeSelf)
            {
                enableCondition = true;
            }

            if (enableCondition)
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

                if (audioSource != null && audioSource.isPlaying)
                {
                    yield return StartCoroutine(FadeOutAudio(audioSource, fadeDuration));
                    audioSource.gameObject.SetActive(false);
                    Debug.Log("Audio source disabled after fade-out.");
                }

                yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator FadeOutAudio(AudioSource source, float duration)
    {
        float startVolume = source.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            source.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        source.Stop();
        source.volume = startVolume; // Reset volume for next use
    }
}