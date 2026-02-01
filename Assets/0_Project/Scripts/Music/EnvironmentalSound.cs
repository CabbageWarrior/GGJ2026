using UnityEngine;

public class EnvironmentAudioController : MonoBehaviour
{
    // Placeholder for wind, rain, ambience logic
    // You can expand this later
    
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if(!audioSource) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
    }

    public void PlayAmbience(AudioClip clip)
    {
        if(audioSource.clip == clip) return;
        audioSource.clip = clip;
        audioSource.Play();
    }
}
