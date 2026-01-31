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
        // 1. Cerchiamo o creiamo un oggetto ESPLICITO per il blocker, senza prendere il primo canvas a caso.
        GameObject blockerGO = GameObject.Find("CurtainBlockerCanvas_World");
        Canvas canvas;

        if (blockerGO == null)
        {
            blockerGO = new GameObject("CurtainBlockerCanvas_World");
            canvas = blockerGO.AddComponent<Canvas>();
            
            // Configurazione WorldSpace
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            
            // Impostiamo una dimensione e posizione sensata per coprire la vista
            // (In WorldSpace il canvas ha una dimensione fisica in metri)
            RectTransform rt = blockerGO.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50); // Grande abbastanza da coprire la visuale
            rt.position = new Vector3(0, 0, 5); // 5 metri davanti alla camera (aggiusta se serve)
            rt.rotation = Quaternion.identity;
            
            // Aggancia alla camera se deve seguire la visuale
            if (Camera.main != null)
            {
                blockerGO.transform.SetParent(Camera.main.transform, false);
                rt.localPosition = new Vector3(0, 0, 1f); // 1 metro davanti alla lente
            }
        }
        else
        {
            canvas = blockerGO.GetComponent<Canvas>();
        }

        // 2. Setup CanvasGroup
        canvasGroup = blockerGO.GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = blockerGO.AddComponent<CanvasGroup>();
        
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false; // Parte SPENTO

        // 3. Setup Raycaster
        graphicRaycaster = blockerGO.GetComponent<GraphicRaycaster>();
        if (graphicRaycaster == null)
            graphicRaycaster = blockerGO.AddComponent<GraphicRaycaster>();
        
        // Questo è importante per bloccare oggetti 3D dietro
        graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.All;

        // 4. Sorting Order (Critico)
        // Deve essere alto (es. 999) per coprire il gioco, 
        // ma il tuo Menu di Pausa dovrà avere 1000 per stare sopra questo.
        canvas.sortingOrder = 999; 
        
        // 5. Aggiungi Immagine "Muro Invisibile"
        // In WorldSpace, senza un'immagine fisica grande quanto il canvas, i click passano attraverso il vuoto!
        Image raycastBlocker = blockerGO.GetComponent<Image>();
        if (raycastBlocker == null) raycastBlocker = blockerGO.AddComponent<Image>();
        raycastBlocker.color = Color.clear; // Invisibile
        raycastBlocker.raycastTarget = true; // Blocca i click
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
