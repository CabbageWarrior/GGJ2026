using UnityEngine;
using System.Collections; // Required for Coroutines

public class LevelMusicPlayer : MonoBehaviour
{
    public AudioManager audioManager;
    [Header("Sequence Settings")]
    public MusicTrack introTrack = MusicTrack.None; // Optional Intro
    public MusicTrack mainLoopTrack; // The actual level music

    [Header("Startup")]
    public bool playOnStart = true;
    public float startDelay = 0f;

    void Start()
    {
        if (playOnStart)
        {
            StartCoroutine(StartMusicSequence());
        }
    }

    IEnumerator StartMusicSequence()
    {
        // 1. Initial Delay (if any)
        if (startDelay > 0) yield return new WaitForSeconds(startDelay);

        // 2. Is there an Intro?
        if (introTrack != MusicTrack.None)
        {
            // A. Play the Intro
            PlayTrack(introTrack);

            // B. Wait for it to finish
            // We need to ask the AudioManager how long this clip is
            float introLength = GetClipLength(introTrack);
            
            // Subtract a tiny bit (0.5s) for a smooth crossfade if you use fades, 
            // otherwise wait the exact length.
            yield return new WaitForSeconds(introLength); 
        }

        // 3. Play the Main Loop
        PlayTrack(mainLoopTrack);
    }

    // --- Helpers ---

    private void PlayTrack(MusicTrack track)
    {
        if (audioManager != null && audioManager.Music != null)
        {
            audioManager.Music.PlayTrack(track);
        }
    }

    // We need to cheat a bit to get the length. 
    // Ideally, AudioManager would tell us, but we can grab it if we have the reference.
    private float GetClipLength(MusicTrack track)
    {
        if (audioManager == null) return 0f;
        
        // Use the internal method from MusicController if public, or a quick switch here
        // Since MusicController.GetClip is private, we rely on a manual check or make it public.
        // For now, let's assume we can add a helper to MusicController or just hardcode/estimate.
        // BETTER SOLUTION: Let's add a public helper to MusicController.cs (Step 2 below).
        return audioManager.Music.GetClipDuration(track); 
    }
}
