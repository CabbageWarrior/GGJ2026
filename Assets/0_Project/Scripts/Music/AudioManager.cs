using UnityEngine;
using UnityEngine.Audio;
using MyGame.Audio;

public class AudioManager : MonoBehaviour
{

    // Sub-Controllers
    public MusicController Music { get; private set; }
    public SFXPool SFX { get; private set; }
    public EnvironmentAudioController Env { get; private set; }
    public UIAudioController UI { get; private set; }
    

    [SerializeField] private AudioMixer audioMixer;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Auto-find children components
        Music = GetComponentInChildren<MusicController>();
        SFX = GetComponentInChildren<SFXPool>();
        Env = GetComponentInChildren<EnvironmentAudioController>();
        UI = GetComponentInChildren<UIAudioController>();
    }

    private void Start()
    {
        // Load saved volumes
        SetVolume("AmbienceVolume", PlayerPrefs.GetFloat("AmbienceVolume", 100f));
        SetVolume("MusicVolume", PlayerPrefs.GetFloat("MusicVolume", 100f));
        SetVolume("SFXVolume", PlayerPrefs.GetFloat("SFXVolume", 100f));
        SetVolume("WetSFXVolume", PlayerPrefs.GetFloat("WetSFXVolume", 100f));
        SetVolume("UIVolume", PlayerPrefs.GetFloat("UIVolume", 100f));
    }

    // --- PUBLIC SHORTCUTS (Call these from your game) ---

    // Music
    public void PlayMenuMusic()      => Music.PlayTrack(MusicTrack.MainMenu);
    public void PlayShortTimeMusic() => Music.PlayTrack(MusicTrack.ShortTime);
    public void PlayScenarioMusic()  => Music.PlayTrack(MusicTrack.MainScenario);
    public void PlayTravelMusic()    => Music.PlayTrack(MusicTrack.Travel);
    public void PlayWinMusic()       => Music.PlayTrack(MusicTrack.Win);
    public void PlayLoseMusic()      => Music.PlayTrack(MusicTrack.Lose);
    // Shortcut to tell the Music Controller to interrupt
    public void PlayInterruption(MusicTrack track) => Music.PlayInterruption(track);
    // Shortcut to tell the Music Controller to resume
    public void ResumeLastMusic() => Music.ResumeMusic();   
    
    // UI
    public void PlayBtnClick()       => UI.PlayClick();
    
    // --- VOLUME LOGIC (Unchanged) ---

    internal float GetAmbienceVolume() => PlayerPrefs.GetFloat("AmbienceVolume", 100f);
    internal float GetMusicVolume() => PlayerPrefs.GetFloat("MusicVolume", 100f);
    internal float GetSFXVolume() => PlayerPrefs.GetFloat("SFXVolume", 100f);

    internal void SetAmbienceVolume(float volume)
    {
        PlayerPrefs.SetFloat("AmbienceVolume", volume);
        PlayerPrefs.Save();
        SetVolume("AmbienceVolume", volume);
    }

    internal void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
        SetVolume("MusicVolume", volume);
    }

    internal void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
        SetVolume("SFXVolume", volume);
        //SetVolume("WetSFXVolume", volume);
        SetVolume("UIVolume", volume);
    }

    private void SetVolume(string parameterName, float volume)
    {
        volume = Mathf.Clamp(volume, 0f, 100f);
        float normalized = volume / 100f;

        if (normalized <= 0.0001f)
        {
            audioMixer.SetFloat(parameterName, -80f);
            return;
        }

        float dB = Mathf.Log10(normalized) * 20f;
        audioMixer.SetFloat(parameterName, dB);
    }
}
