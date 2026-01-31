using UnityEngine;
using UnityEngine.Events;

public class PreShuffleTimer : MonoBehaviour
{
    [Header("Timing Settings")]
    [Tooltip("Initial duration of the pre-shuffle phase (in seconds).")]
    public float initialDuration = 10f;

    [Tooltip("How much the pre-shuffle duration is reduced after each round (in seconds).")]
    public float durationDecrease = 1f;

    [Tooltip("Minimum duration that the pre-shuffle timer can reach (in seconds).")]
    public float minDuration = 2f;

    [Tooltip("If true, the pre-shuffle timer starts automatically when this object is enabled.")]
    public bool autoStartOnEnable = false;

    [Header("Events")]
    [Tooltip("Invoked when the pre-shuffle phase starts (player sees the demons before they move).")]
    public UnityEvent onPreShuffleStart;

    [Tooltip("Invoked every frame while the pre-shuffle timer is running (use this to update UI, sounds, etc.).")]
    public UnityEvent onPreShuffleTick;

    [Tooltip("Invoked when the pre-shuffle timer ends. Typically used to start the shuffle/mixing phase.")]
    public UnityEvent onPreShuffleEnd;

    [Header("Debug Info (Read Only)")]
    [Tooltip("Current duration of the pre-shuffle phase for this round (in seconds).")]
    public float currentDuration;

    [Tooltip("Current time remaining for this pre-shuffle phase (in seconds).")]
    public float timeRemaining;

    bool isRunning = false;

    void Awake()
    {
        // Initialize from inspector value
        currentDuration = initialDuration;
    }

    void OnEnable()
    {
        if (autoStartOnEnable)
            StartPreShufflePhase();
    }

    void Update()
    {
        if (!isRunning) return;

        timeRemaining -= Time.deltaTime;   // standard countdown 
        onPreShuffleTick?.Invoke();

        if (timeRemaining <= 0f)
        {
            isRunning = false;
            timeRemaining = 0f;
            onPreShuffleEnd?.Invoke();
        }
    }

    /// Starts the pre-shuffle phase timer (players are recognizing the demons).
    /// Call this at the beginning of the "feet to the fire" phase.
    public void StartPreShufflePhase()
    {
        isRunning = true;
        timeRemaining = currentDuration;
        onPreShuffleStart?.Invoke();
    }

    /// Prepares the timer for the next round/hut.
    /// Call this once after the whole loop (pre-shuffle + shuffle + choice) is completed.
    public void PrepareNextRound()
    {
        currentDuration = Mathf.Max(minDuration, currentDuration - durationDecrease);
    }

    /// Returns the remaining time for the current pre-shuffle phase.
    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    /// Returns the configured duration for the current round.
    public float GetCurrentDuration()
    {
        return currentDuration;
    }
}
