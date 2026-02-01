using UnityEngine;

public class LevelMusicPlayer : MonoBehaviour
{
    [Header("Startup Music")]
    public MusicTrack trackToPlay; 
    public bool playOnStart = true;
    public float delay = 0f;

    void Start()
    {
        if (playOnStart)
        {
            if (delay > 0)
                Invoke(nameof(PlayConfiguredTrack), delay);
            else
                PlayConfiguredTrack();
        }
    }

    // Call this internally for the startup logic
    void PlayConfiguredTrack()
    {
        PlayTrack(trackToPlay);
    }

    // --- PUBLIC METHODS (Call these from Triggers/Buttons/Timeline) ---

    // 1. Generic function to play the track set in the Inspector
    public void PlayCurrentTrack()
    {
        PlayTrack(trackToPlay);
    }

    // 2. Specific function to switch to ANY track dynamically
    // Useful for UnityEvents where you can pick the enum from the list
    public void SwitchToTrack(MusicTrack newTrack)
    {
        PlayTrack(newTrack);
    }
    
    // Helper function
    private void PlayTrack(MusicTrack track)
    {
        if (AudioManager.Instance != null && AudioManager.Instance.Music != null)
        {
            AudioManager.Instance.Music.PlayTrack(track);
        }
    }
}
