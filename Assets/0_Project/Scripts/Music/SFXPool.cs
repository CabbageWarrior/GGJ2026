using UnityEngine;
using UnityEngine.Audio; // Required for AudioMixerGroup
using System.Collections.Generic;

public class SFXPool : MonoBehaviour
{
    [Header("Pool Settings")]
    public int poolSize = 15; // Increased to 15 to be safe
    
    [Header("Mixer Routing")]
    public AudioMixerGroup sfxMixerGroup; // Drag your 'SFX' group here in Inspector

    private List<AudioSource> sources;
    private int currentIndex = 0;

    void Awake()
    {
        sources = new List<AudioSource>();

        // Create a pool of AudioSources so we can play multiple sounds at once
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = new GameObject("SFX_Source_" + i);
            obj.transform.SetParent(transform); // Keep hierarchy clean
            
            AudioSource src = obj.AddComponent<AudioSource>();
            src.playOnAwake = false;
            
            // --- NEW: Route to Mixer ---
            // This ensures the sound goes through the "SFX" volume slider
            if (sfxMixerGroup != null)
            {
                src.outputAudioMixerGroup = sfxMixerGroup;
            }
            
            sources.Add(src);
        }
    }

    /// <summary>
    /// Plays a sound effect from the pool.
    /// </summary>
    /// <param name="clip">The audio file to play</param>
    /// <param name="volume">Volume (0 to 1)</param>
    /// <param name="pitch">Pitch/Speed (1 is normal)</param>
    public void Play(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (clip == null) return;

        // Find the next available source in the list
        AudioSource source = sources[currentIndex];
        
        // Setup parameters
        source.pitch = pitch;
        
        // PlayOneShot allows multiple overlapping sounds on the same source if needed,
        // but rotating through sources is cleaner for volume control.
        source.PlayOneShot(clip, volume);

        // Move to next index (Circular buffer logic)
        currentIndex++;
        if (currentIndex >= sources.Count) 
        {
            currentIndex = 0;
        }
    }
}
