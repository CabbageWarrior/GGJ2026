using UnityEngine;
using DG.Tweening; // Ensure DOTween is imported!

public enum MusicTrack
{
    None,
    MainMenu,
    ShortTime,
    MainScenario,
    Travel,
    Win,
    Lose
}

public class MusicController : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip menuTheme;
    public AudioClip shortTimeTheme;
    public AudioClip scenarioTheme;
    public AudioClip travelTheme;
    public AudioClip winTheme;
    public AudioClip loseTheme;

    [Header("Loop Settings")]
    public bool loopMainMenu = true;
    public bool loopShortTime = true;
    public bool loopScenario = true;
    public bool loopTravel = true;
    public bool loopWin = false;  // Usually Win/Lose play once
    public bool loopLose = false;

    [Header("Fading")]
    public float fadeDuration = 1.0f;
    
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource) audioSource = gameObject.AddComponent<AudioSource>();
        
        // Ensure it doesn't play on awake
        audioSource.playOnAwake = false;
    }

    public void PlayTrack(MusicTrack track)
    {
        AudioClip nextClip = GetClip(track);
        bool shouldLoop = GetLoopSetting(track);

        // If asking for the same track that is already playing, just ensure loop is correct
        if (audioSource.clip == nextClip && audioSource.isPlaying) 
        {
            audioSource.loop = shouldLoop;
            return; 
        }

        // FADE OUT -> SWITCH -> FADE IN
        audioSource.DOFade(0f, fadeDuration).SetUpdate(true).OnComplete(() =>
        {
            audioSource.clip = nextClip;
            audioSource.loop = shouldLoop;
            
            if (nextClip != null)
            {
                audioSource.Play();
                audioSource.DOFade(1f, fadeDuration).SetUpdate(true);
            }
        });
    }

    private AudioClip GetClip(MusicTrack track)
    {
        switch (track)
        {
            case MusicTrack.MainMenu:     return menuTheme;
            case MusicTrack.ShortTime:    return shortTimeTheme;
            case MusicTrack.MainScenario: return scenarioTheme;
            case MusicTrack.Travel:       return travelTheme;
            case MusicTrack.Win:          return winTheme;
            case MusicTrack.Lose:         return loseTheme;
            default: return null;
        }
    }

    private bool GetLoopSetting(MusicTrack track)
    {
        switch (track)
        {
            case MusicTrack.MainMenu:     return loopMainMenu;
            case MusicTrack.ShortTime:    return loopShortTime;
            case MusicTrack.MainScenario: return loopScenario;
            case MusicTrack.Travel:       return loopTravel;
            case MusicTrack.Win:          return loopWin;
            case MusicTrack.Lose:         return loopLose;
            default: return false;
        }
    }
}
