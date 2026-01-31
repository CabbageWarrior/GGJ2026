using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using System.Collections;

public class TimelineCurtainBlocker : MonoBehaviour
{
    [Header("Curtain Sprite Loaders")]
    public SpriteRenderer[] leftCurtainSprites;
    public SpriteRenderer[] rightCurtainSprites;
    public Sprite[] curtainSprites;

    [Header("Curtain Positions (3D World Space)")]
    // Posizione APERTA (Gameplay)
    public Vector3 leftCurtainOpenPos = new Vector3(-15, 0, 5);
    public Vector3 rightCurtainOpenPos = new Vector3(15, 0, 5);
    
    // Posizione CHIUSA (Pausa)
    public Vector3 curtainClosedPos = Vector3.zero;
    
    public float curtainDuration = 1.5f;

    [Header("Invisible Blocker")]
    public Image blockerImage;

    [Header("Events")]
    public UnityEvent onCurtainsClosed;   // Evento lanciato quando le tende sono chiuse
    public UnityEvent onCurtainsOpened;   // Evento lanciato quando le tende sono aperte

    CanvasGroup canvasGroup;
    GraphicRaycaster graphicRaycaster;

    void Awake()
    {
        SetupBlocker();
    }

    void SetupBlocker()
    {
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasGO = new GameObject("TimelineCanvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
        }

        canvasGroup = canvas.gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;

        graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
        if (graphicRaycaster == null)
            graphicRaycaster = canvas.gameObject.AddComponent<GraphicRaycaster>();
        graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.All;

        canvas.sortingOrder = 999;
    }
    
    void Start()
    {
        // Al via del gioco, assicuriamoci che le tende siano APERTE e invisibili
        foreach (var sprite in leftCurtainSprites)
            if(sprite) sprite.transform.localPosition = leftCurtainOpenPos;

        foreach (var sprite in rightCurtainSprites)
            if(sprite) sprite.transform.localPosition = rightCurtainOpenPos;

        if(canvasGroup) 
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }
    }

    // --- METODI PUBBLICI DA COLLEGARE AGLI EVENTI UNITY ---

    // Chiama questo su "On Enter Paused"
    public void CloseCurtains()
    {
        StartCoroutine(CloseSequence());
    }

    // Chiama questo su "On Enter Gameplay" (o "On Exit Paused")
    public void OpenCurtains()
    {
        StartCoroutine(OpenSequence());
    }

    // ------------------------------------------------------

    IEnumerator CloseSequence()
    {
        // 1. Attiva blocker
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        
        // 2. Carica sprite (opzionale, se cambiano)
        LoadCurtainSprites();

        // 3. Muovi verso CENTRO (0,0,0) - USARE SetUpdate(true) per TimeScale=0
        foreach (var sprite in leftCurtainSprites)
            if(sprite) sprite.transform.DOLocalMove(curtainClosedPos, curtainDuration).SetEase(Ease.InOutQuad).SetUpdate(true);
            
        foreach (var sprite in rightCurtainSprites)
            if(sprite) sprite.transform.DOLocalMove(curtainClosedPos, curtainDuration).SetEase(Ease.InOutQuad).SetUpdate(true);

        // USARE WaitForSecondsRealtime per TimeScale=0
        yield return new WaitForSecondsRealtime(curtainDuration);

        onCurtainsClosed?.Invoke();
    }

    IEnumerator OpenSequence()
    {
        // 1. Muovi verso LATI (Posizione Open) - USARE SetUpdate(true) per TimeScale=0
        foreach (var sprite in leftCurtainSprites)
            if(sprite) sprite.transform.DOLocalMove(leftCurtainOpenPos, curtainDuration).SetEase(Ease.InOutQuad).SetUpdate(true);

        foreach (var sprite in rightCurtainSprites)
            if(sprite) sprite.transform.DOLocalMove(rightCurtainOpenPos, curtainDuration).SetEase(Ease.InOutQuad).SetUpdate(true);

        // USARE WaitForSecondsRealtime per TimeScale=0
        yield return new WaitForSecondsRealtime(curtainDuration);

        // 2. Disattiva blocker
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0f, 0.3f).SetUpdate(true);
        yield return new WaitForSecondsRealtime(0.3f);
        canvasGroup.blocksRaycasts = false;

        onCurtainsOpened?.Invoke();
    }

    void LoadCurtainSprites()
    {
        for (int i = 0; i < leftCurtainSprites.Length && i < curtainSprites.Length; i++)
        {
            if (leftCurtainSprites[i]) leftCurtainSprites[i].sprite = curtainSprites[i];
            if (rightCurtainSprites[i] && i < rightCurtainSprites.Length)
                rightCurtainSprites[i].sprite = curtainSprites[i];
        }
    }
}
