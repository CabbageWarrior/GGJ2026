using UnityEngine;

public class UIAudioController : MonoBehaviour
{
    [Header("UI Clips")]
    public AudioClip buttonClick;
    
    // Add more here if needed (e.g., Hover, Back, Error)
    // public AudioClip buttonHover;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource) audioSource = gameObject.AddComponent<AudioSource>();
        
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    public void PlayClick()
    {
        PlayOneShot(buttonClick);
    }

    private void PlayOneShot(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            // Optional: minimal pitch variation to make it sound organic
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(clip);
        }
    }
}