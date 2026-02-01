using UnityEngine;

public class SpecialEventMusic : MonoBehaviour
{
    [Header("Configuration")]
    public MusicTrack eventMusic = MusicTrack.ShortTime;
    public float eventDuration = 5f; // Optional auto-resume

    public void StartEvent()
    {
        // "Hey Brain, save the current song and play this special track!"
        AudioManager.Instance.PlayInterruption(eventMusic);
        
        // Option A: Auto-resume after X seconds
        Invoke(nameof(EndEvent), eventDuration);
    }

    public void EndEvent()
    {
        // "Hey Brain, the event is over. Go back to normal."
        AudioManager.Instance.ResumeLastMusic();
    }
}
