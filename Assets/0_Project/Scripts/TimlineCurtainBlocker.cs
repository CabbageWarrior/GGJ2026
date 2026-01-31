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
    public Vector3 leftCurtainOpenPos = new Vector3(-15, 0, 0);
    public Vector3 rightCurtainOpenPos = new Vector3(15, 0, 0);
    
    // Posizione CHIUSA (Pausa)
    public Vector3 leftCurtainClosedPos = new Vector3(3.5f, 0, 0);
    public Vector3 rightCurtainClosedPos = new Vector3(-3.5f, 0, 0);
    
    public float curtainDuration = 1.5f;

    [Header("Final Image Transition")]
    public Image finalImage;
    public float imageFadeDuration = 0.5f;

    [Header("Invisible Blocker")]
    public Image blockerImage;

    [Header("Events")]
    public UnityEvent onCurtainsClosed;
    public UnityEvent onCurtainsOpened;

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
        foreach (var sprite in leftCurtainSprites)
            if(sprite) sprite.transform.localPosition = leftCurtainOpenPos;

        foreach (var sprite in rightCurtainSprites)
            if(sprite) sprite.transform.localPosition = rightCurtainOpenPos;

        if (finalImage) 
        {
            Color c = finalImage.color;
            c.a = 0f;
            finalImage.color = c;
            finalImage.gameObject.SetActive(false);
        }

        if(canvasGroup) 
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void CloseCurtains()
    {
        StartCoroutine(CloseSequence());
    }

    public void OpenCurtains()
    {
        StartCoroutine(OpenSequence());
    }

    // UNICO METODO CloseSequence (Ease.InCubic per accelerazione)
    IEnumerator CloseSequence()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        
        LoadCurtainSprites();

        foreach (var sprite in leftCurtainSprites)
            if(sprite) sprite.transform.DOLocalMove(leftCurtainClosedPos, curtainDuration).SetEase(Ease.InCubic).SetUpdate(true);
            
        foreach (var sprite in rightCurtainSprites)
            if(sprite) sprite.transform.DOLocalMove(rightCurtainClosedPos, curtainDuration).SetEase(Ease.InCubic).SetUpdate(true);

        yield return new WaitForSecondsRealtime(curtainDuration);

        if (finalImage)
        {
            finalImage.gameObject.SetActive(true);
            finalImage.DOFade(1f, imageFadeDuration).SetUpdate(true);
            yield return new WaitForSecondsRealtime(imageFadeDuration);
        }

        onCurtainsClosed?.Invoke();
    }

    IEnumerator OpenSequence()
    {
        if (finalImage)
        {
            finalImage.DOFade(0f, imageFadeDuration).SetUpdate(true);
            yield return new WaitForSecondsRealtime(imageFadeDuration);
            finalImage.gameObject.SetActive(false);
        }

        foreach (var sprite in leftCurtainSprites)
            if(sprite) sprite.transform.DOLocalMove(leftCurtainOpenPos, curtainDuration).SetEase(Ease.InOutQuad).SetUpdate(true);

        foreach (var sprite in rightCurtainSprites)
            if(sprite) sprite.transform.DOLocalMove(rightCurtainOpenPos, curtainDuration).SetEase(Ease.InOutQuad).SetUpdate(true);

        yield return new WaitForSecondsRealtime(curtainDuration);

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
