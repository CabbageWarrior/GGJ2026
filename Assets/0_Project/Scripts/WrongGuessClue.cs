using UnityEngine;
using UnityEngine.Events;

public class WrongGuessClue : MonoBehaviour
{
    [Header("Clue Settings")]
    [Tooltip("Duration of the clue flash in seconds (keep it short: 0.5-1s).")]
    public float clueDuration = 0.8f;

    [Tooltip("Color to flash the guilty demons (bright red/orange for visibility).")]
    public Color guiltyFlashColor = Color.red;

    [Tooltip("How many times to flash during the clue duration.")]
    public int flashCount = 3;

    [Header("Events")]
    [Tooltip("Call this when player guesses wrong. Pass the guilty demons.")]
    public UnityEvent<Transform[]> onShowClue;  // Not used, public method instead

    [Tooltip("Called when clue animation completes.")]
    public UnityEvent onClueComplete;

    private Renderer[] guiltyRenderers;
    private Material[] originalMaterials;
    private Material flashMaterial;
    private float flashTimer;
    private bool isShowingClue = false;
    private int currentFlash = 0;

    void Awake()
    {
        // Create a temporary flash material
        flashMaterial = new Material(Shader.Find("Guess"));  // or sprite shader 
        flashMaterial.color = guiltyFlashColor;
    }

    void Update()
    {
        if (!isShowingClue) return;

        flashTimer -= Time.deltaTime;

        // Flash logic: alternate between normal and flash [web:62]
        bool showFlash = (int)(flashTimer * flashCount * 2) % 2 == 0;
        SetGuiltyFlash(showFlash);

        if (flashTimer <= 0f)
        {
            isShowingClue = false;
            RestoreOriginalMaterials();
            onClueComplete?.Invoke();  // Proceed to next phase or lose life
        }
    }

    /// Call this from your GameManager when player guesses wrong.
    /// Pass the array of TRUE guilty demons to flash as clue.
    public void ShowGuiltyClue(Transform[] guiltyDemons)
    {
        guiltyRenderers = new Renderer[guiltyDemons.Length];
        originalMaterials = new Material[guiltyDemons.Length];

        for (int i = 0; i < guiltyDemons.Length; i++)
        {
            Renderer rend = guiltyDemons[i].GetComponent<Renderer>();
            if (rend != null)
            {
                guiltyRenderers[i] = rend;
                originalMaterials[i] = rend.material;
            }
        }

        isShowingClue = true;
        flashTimer = clueDuration;
        currentFlash = 0;
    }

    private void SetGuiltyFlash(bool flashOn)
    {
        for (int i = 0; i < guiltyRenderers.Length; i++)
        {
            if (guiltyRenderers[i] != null)
            {
                if (flashOn)
                    guiltyRenderers[i].material = flashMaterial;
                else
                    guiltyRenderers[i].material = originalMaterials[i];
            }
        }
    }

    private void RestoreOriginalMaterials()
    {
        for (int i = 0; i < guiltyRenderers.Length; i++)
        {
            if (guiltyRenderers[i] != null)
                guiltyRenderers[i].material = originalMaterials[i];
        }
    }

    void OnDestroy()
    {
        if (flashMaterial != null)
            DestroyImmediate(flashMaterial);
    }
}
