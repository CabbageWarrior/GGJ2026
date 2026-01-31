using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class PeopleShuffle : MonoBehaviour
{
    [Header("Shuffle Between Spaces")]
    [Tooltip("Spaces/positions where people will move to during shuffle.")]
    public Transform[] shuffleSpaces;

    [Tooltip("Tag of all people to auto-detect and shuffle.")]
    public string peopleTag = "People";

    [Tooltip("Duration of smooth movement between spaces (seconds).")]
    public float shuffleDuration = 2f;

    [Tooltip("Auto-start shuffle when this GameObject is enabled.")]
    public bool autoStartOnEnable = false;

    [Header("Events")]
    public UnityEvent onShuffleStart;
    public UnityEvent onShuffleTick;
    public UnityEvent onShuffleComplete;

    [Header("Debug")]
    public int peopleCount;
    public int spacesCount;
    public float shuffleProgress;

    private List<Transform> allPeople;
    private Dictionary<Transform, Vector3> originalPositions;
    private bool isShuffling = false;

    void OnEnable()
    {
        if (autoStartOnEnable)
            StartShuffle();
    }

    /// <summary>
    /// Auto-finds people by tag, shuffles them between your predefined spaces.
    /// </summary>
    public void StartShuffle()
    {
        // AUTO FIND people by tag
        GameObject[] peopleObjects = GameObject.FindGameObjectsWithTag(peopleTag);
        allPeople = peopleObjects.Select(p => p.transform).ToList();
        peopleCount = allPeople.Count;
        spacesCount = shuffleSpaces.Length;

        if (peopleCount == 0)
        {
            Debug.LogWarning($"No people found with tag '{peopleTag}'!");
            return;
        }

        if (spacesCount == 0)
        {
            Debug.LogWarning("No shuffle spaces assigned!");
            return;
        }

        // Store original positions
        originalPositions = new Dictionary<Transform, Vector3>();
        foreach (var person in allPeople)
            originalPositions[person] = person.position;

        // Fisher-Yates shuffle: assign people to spaces randomly [web:33]
        allPeople.Shuffle();

        isShuffling = true;
        shuffleProgress = 0f;
        onShuffleStart?.Invoke();
    }

    void Update()
    {
        if (!isShuffling) return;

        shuffleProgress += Time.deltaTime / shuffleDuration;
        onShuffleTick?.Invoke();

        // Move each person to their assigned shuffle space
        for (int i = 0; i < allPeople.Count; i++)
        {
            if (i < shuffleSpaces.Length)
            {
                Vector3 targetSpace = shuffleSpaces[i].position;
                allPeople[i].position = Vector3.Lerp(
                    originalPositions[allPeople[i]], 
                    targetSpace, 
                    shuffleProgress
                );
            }
        }

        if (shuffleProgress >= 1f)
        {
            isShuffling = false;
            shuffleProgress = 1f;
            onShuffleComplete?.Invoke();
        }
    }

    public bool IsShuffling()
    {
        return isShuffling;
    }
}

// Shuffle extension
public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
