using UnityEngine;

namespace MyGame.Audio // Added namespace to avoid conflicts
{
  
    public class UIAudioController : MonoBehaviour
    {
        public AudioManager audioManager;
        [Header("UI Audio Clips")]
        public AudioClip clickSound;
        public AudioClip hoverSound;

        [Header("Settings")]
        [Range(0f, 3f)] public float volume = 1f;

        // We use the SFX Pool to play sounds so rapid clicks don't cut each other off
        public void PlayClick()
        {
            if (clickSound == null) return;
            // Access the global AudioManager to play via the SFX pool
            audioManager.SFX.Play(clickSound, volume);
        }

        public void PlayHover()
        {
            if (hoverSound == null) return;
            audioManager.SFX.Play(hoverSound, volume); // Slightly quieter
        }
    }
}
