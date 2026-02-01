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
    public bool loopWin = false;
    public bool loopLose = false;

    [Header("Fading")]
    public float fadeDuration = 1.0f;

    private AudioSource audioSource;
    private MusicTrack _trackBeforeInterruption = MusicTrack.None; // MEMORY

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource) audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }

    // --- MAIN PLAY FUNCTION ---
    public void PlayTrack(MusicTrack track)
    {
        AudioClip nextClip = GetClip(track);
        bool shouldLoop = GetLoopSetting(track);

        // Optimization: If same track, just update loop setting
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

    // --- INTERRUPTION SYSTEM ---
    public void PlayInterruption(MusicTrack temporaryTrack)
    {
        // 1. Remember what is currently playing
        if (_trackBeforeInterruption == MusicTrack.None) 
        {
            // Only save if we aren't ALREADY in an interruption (don't overwrite with "None")
            _trackBeforeInterruption = GetCurrentTrack();
        }

        // 2. Play the new track (e.g., Battle Music, Item Fanfare)
        PlayTrack(temporaryTrack);
    }

    public void ResumeMusic()
    {
        // 3. Check memory
        if (_trackBeforeInterruption != MusicTrack.None)
        {
            PlayTrack(_trackBeforeInterruption);
            _trackBeforeInterruption = MusicTrack.None; // Clear memory
        }
    }

    // --- HELPERS ---

    // REQUIRED for LevelMusicPlayer to know how long an intro is
    public float GetClipDuration(MusicTrack track)
    {
        AudioClip clip = GetClip(track);
        return clip != null ? clip.length : 0f;
    }

    private MusicTrack GetCurrentTrack()
    {
        if (audioSource.clip == menuTheme) return MusicTrack.MainMenu;
        if (audioSource.clip == shortTimeTheme) return MusicTrack.ShortTime;
        if (audioSource.clip == scenarioTheme) return MusicTrack.MainScenario;
        if (audioSource.clip == travelTheme) return MusicTrack.Travel;
        if (audioSource.clip == winTheme) return MusicTrack.Win;
        if (audioSource.clip == loseTheme) return MusicTrack.Lose;
        return MusicTrack.None;
    }

    private AudioClip GetClip(MusicTrack track)
    {
        switch (track)
        {
            case MusicTrack.MainMenu: return menuTheme;
            case MusicTrack.ShortTime: return shortTimeTheme;
            case MusicTrack.MainScenario: return scenarioTheme;
            case MusicTrack.Travel: return travelTheme;
            case MusicTrack.Win: return winTheme;
            case MusicTrack.Lose: return loseTheme;
            default: return null;
        }
    }

    private bool GetLoopSetting(MusicTrack track)
    {
        switch (track)
        {
            case MusicTrack.MainMenu: return loopMainMenu;
            case MusicTrack.ShortTime: return loopShortTime;
            case MusicTrack.MainScenario: return loopScenario;
            case MusicTrack.Travel: return loopTravel;
            case MusicTrack.Win: return loopWin;
            case MusicTrack.Lose: return loopLose;
            default: return false;
        }
    }
}
