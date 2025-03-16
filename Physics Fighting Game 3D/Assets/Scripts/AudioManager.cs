using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip loopingClip;   // Audio clip to loop after the nuke hits

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found on the AudioManager!");
        }
    }

    public void PlayLoopingAudio()
    {
        if (audioSource != null && loopingClip != null)
        {
            audioSource.clip = loopingClip;
            audioSource.loop = true; // Enable looping
            audioSource.Play();
            Debug.Log("Audio file is now looping.");
        }
        else
        {
            Debug.LogError("AudioSource or LoopingClip not assigned!");
        }
    }
}
