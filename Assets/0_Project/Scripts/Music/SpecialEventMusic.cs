using UnityEngine;

public class SpecialEventMusic : MonoBehaviour
{
    public AudioManager audioManager;
    [Header("Configuration")]
    public MusicTrack eventMusic = MusicTrack.ShortTime;
    public float eventDuration = 5f; // Optional auto-resume

    public void StartEvent()
    {
        // "Hey Brain, save the current song and play this special track!"
        audioManager.PlayInterruption(eventMusic);
        
        // Option A: Auto-resume after X seconds
        Invoke(nameof(EndEvent), eventDuration);
    }

    public void EndEvent()
    {
        // "Hey Brain, the event is over. Go back to normal."
        audioManager.ResumeLastMusic();
    }
}
