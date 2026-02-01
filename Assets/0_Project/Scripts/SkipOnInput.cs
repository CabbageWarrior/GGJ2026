using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipOnInput : MonoBehaviour
{
    [Header("Configuration")]
    public string nextSceneName; // Type the exact scene name in Inspector
    public KeyCode skipKey = KeyCode.Escape;

    void Update()
    {
        if (Input.GetKeyDown(skipKey))
        {
            SkipScene();
        }
    }

    void SkipScene()
    {
        // Optional: Stop music immediately?
        // if (AudioManager.Instance != null) AudioManager.Instance.Music.PlayTrack(MusicTrack.None);

        SceneManager.LoadScene(2);
    }
}