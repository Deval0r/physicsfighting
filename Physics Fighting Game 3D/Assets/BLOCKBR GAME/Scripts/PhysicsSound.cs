using UnityEngine;

public class PhysicsSound : MonoBehaviour
{
    public AudioClip tapSound;
    public AudioClip bonkSound;
    
    [Range(0.1f, 2f)]
    public float pitchVariation = 0.2f;
    public float minForceThreshold = 0.1f;
    public float maxForceThreshold = 10f;
    
    private AudioSource audioSource;
    private bool wasInAir = false;
    private float minPitch = 0.8f;
    private float maxPitch = 1.2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f; // Make the sound 3D
    }

    void OnCollisionEnter(Collision collision)
    {
        float impactForce = collision.impulse.magnitude;
        
        if (impactForce < minForceThreshold) return;

        // Calculate volume based on impact force
        float volume = Mathf.Clamp01(impactForce / maxForceThreshold);
        
        // Randomize pitch
        float randomPitch = Random.Range(minPitch, maxPitch);

        // Check if this was a landing from a jump
        if (wasInAir && Vector3.Dot(collision.contacts[0].normal, Vector3.up) > 0.7f)
        {
            PlaySound(bonkSound, volume, randomPitch);
        }
        wasInAir = false;
    }

    void OnCollisionStay(Collision collision)
    {
        // Check for rolling/edge hits
        foreach (ContactPoint contact in collision.contacts)
        {
            // If contact normal is not mostly vertical, it's likely an edge hit
            if (Mathf.Abs(Vector3.Dot(contact.normal, Vector3.up)) < 0.7f)
            {
                float impactForce = collision.impulse.magnitude;
                if (impactForce > minForceThreshold)
                {
                    float volume = Mathf.Clamp01(impactForce / maxForceThreshold);
                    float randomPitch = Random.Range(minPitch, maxPitch);
                    PlaySound(tapSound, volume * 0.5f, randomPitch);
                }
                break;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Mark that we've left the ground
        wasInAir = true;
    }

    private void PlaySound(AudioClip clip, float volume, float pitch)
    {
        // Prevent sound spam by not playing if another sound is too recent
        if (!audioSource.isPlaying || Time.time - audioSource.time > 0.1f)
        {
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(clip, volume);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
