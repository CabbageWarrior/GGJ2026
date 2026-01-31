using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.SceneManagement; // AGGIUNTO: Serve per il Restart (ricaricare la scena)

public enum GameState
{
    Gameplay,
    Paused
}

public class PauseManager : MonoBehaviour
{
    [Header("References")]
    public TimelineCurtainBlocker curtainBlocker;
    public GameObject pauseMenuUI;

    [Header("State Events")]
    public UnityEvent onEnterGameplay;
    public UnityEvent onExitGameplay;
    public UnityEvent onEnterPaused;
    public UnityEvent onExitPaused;
    public UnityEvent<GameState> onStateChanged;

    public GameState CurrentState { get; private set; } = GameState.Gameplay;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause(); 
        }
    }

    // Metodo helper per cambiare stato al volo (utile anche da chiamare esternamente se serve)
    public void TogglePause()
    {
        Debug.Log("Toggle Pause Richiamato! Stato attuale: " + CurrentState);

        if (CurrentState == GameState.Gameplay)
        {
            Debug.Log("→ ENTRA PAUSE");
            SetState(GameState.Paused);
        }
        else if (CurrentState == GameState.Paused)
        {
            Debug.Log("→ ESCI PAUSE");
            SetState(GameState.Gameplay);
        }
    }

    

    
    public void OnResumeButton()
    {
        // Torna semplicemente allo stato Gameplay
        SetState(GameState.Gameplay);
    }

    // Collegalo al bottone "RESTART"
    public void OnRestartButton()
    {
        // Importante: ripristina il tempo prima di ricaricare, altrimenti la nuova scena parte freezata!
        Time.timeScale = 1f;
        
        // Ricarica la scena attuale
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Collegalo al bottone "QUIT"
    public void OnQuitButton()
    {
        Debug.Log("Uscita dal gioco...");
        Application.Quit();

        // Utile per testare l'uscita anche nell'Editor di Unity
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    // --- FINE NUOVE FUNZIONI ---

    public void SetState(GameState newState)
    {
        if (newState == CurrentState) return;

        // EXIT present state
        switch (CurrentState)
        {
            case GameState.Gameplay:
                onExitGameplay?.Invoke();
                break;
            case GameState.Paused:
                onExitPaused?.Invoke();
                break;
        }

        // Change State
        CurrentState = newState;
        onStateChanged?.Invoke(CurrentState);

        // ENTER new state
        switch (CurrentState)
        {
            case GameState.Gameplay:
                EnterGameplay();
                break;
            case GameState.Paused:
                EnterPaused();
                break;
        }
    }

    void EnterPaused() 
    {
        Time.timeScale = 0f;
        if (pauseMenuUI) pauseMenuUI.SetActive(true);
        if (curtainBlocker) curtainBlocker.CloseCurtains();
        
        onEnterPaused?.Invoke();
    }

    void EnterGameplay() 
    {
        Time.timeScale = 1f;
        if (pauseMenuUI) pauseMenuUI.SetActive(false);
        if (curtainBlocker) curtainBlocker.OpenCurtains();
        
        onEnterGameplay?.Invoke();
    }
}
