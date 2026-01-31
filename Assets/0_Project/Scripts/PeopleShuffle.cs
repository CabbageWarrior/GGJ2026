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

    public int badPeople = 1;

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

    private List<PeasantData> allPeoplePeasantData;

    private GameManager gm;

    private GameManager GM
    {
        get
        {
            if (gm == null)
                gm = FindFirstObjectByType<GameManager>();
            return gm;
        }
    }

    void OnEnable()
    {
        if (autoStartOnEnable)
            StartShuffle();
    }

    internal void SetupShuffle(GameObject peasantPrefab, PeasantPartsScriptable peasantParts)
    {
        allPeople = new List<Transform>();
        allPeoplePeasantData = new List<PeasantData>();

        for (int i = 0; i < shuffleSpaces.Length; i++)
        {
            PeasantData newPeasantData = new PeasantData();
            newPeasantData.isTarget = i < badPeople;
            do
            {
                newPeasantData.patternId = UnityEngine.Random.Range(0, peasantParts.patternMasks.Count);
                newPeasantData.baseColor = peasantParts.baseColors[UnityEngine.Random.Range(0, peasantParts.baseColors.Count)];
                newPeasantData.patternColor = peasantParts.patternColors[UnityEngine.Random.Range(0, peasantParts.patternColors.Count)];
            }
            while (allPeoplePeasantData.Any(p =>
                p.patternId == newPeasantData.patternId &&
                p.baseColor == newPeasantData.baseColor &&
                p.patternColor == newPeasantData.patternColor));

            allPeoplePeasantData.Add(newPeasantData);
        }

        allPeoplePeasantData.Shuffle();

        for (int i = 0; i < shuffleSpaces.Length; i++)
        {
            GameObject newPeasant = Instantiate(peasantPrefab, shuffleSpaces[i].position, Quaternion.identity, this.transform);
            newPeasant.GetComponent<Peasant>().Init(GM, allPeoplePeasantData[i]);
            allPeople.Add(newPeasant.transform);
        }
    }

    /// <summary>
    /// Auto-finds people by tag, shuffles them between your predefined spaces.
    /// </summary>
    public void StartShuffle()
    {
        // AUTO FIND people by tag
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

        PutShoesOn();

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

    public void PutShoesOn()
    {
        foreach (var person in allPeople)
        {
            Peasant peasantComponent = person.GetComponent<Peasant>();
            if (peasantComponent != null)
            {
                peasantComponent.HideFeet();
            }
        }
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
