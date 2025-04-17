using UnityEngine;

public class NukeSound : MonoBehaviour
{
    // Assign the audio clip in the Inspector
    public AudioClip soundClip;
    private AudioSource audioSource;

    void Start()
    {
        // Add an AudioSource component to the GameObject if not already present
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = soundClip;

        // Play the sound as soon as the scene starts
        audioSource.Play();
    }
}