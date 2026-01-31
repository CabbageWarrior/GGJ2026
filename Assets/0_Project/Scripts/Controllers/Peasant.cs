using UnityEngine;
using UnityEngine.EventSystems;

public class Peasant : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public SpriteRenderer expressionRenderer;

    public SpriteRenderer bodyRenderer;
    public SpriteMask patternMask;
    public SpriteRenderer patternRenderer;
    
    public SpriteRenderer armUp1Renderer;
    public SpriteRenderer armUp2Renderer;
    public SpriteMask armUp1PatternMask;
    public SpriteMask armUp2PatternMask;
    public SpriteRenderer armUp1PatternRenderer;
    public SpriteRenderer armUp2PatternRenderer;

    public SpriteRenderer armDown1Renderer;
    public SpriteRenderer armDown2Renderer;
    public SpriteMask armDown1PatternMask;
    public SpriteMask armDown2PatternMask;
    public SpriteRenderer armDown1PatternRenderer;
    public SpriteRenderer armDown2PatternRenderer;

    public SpriteRenderer feetRenderer1;
    public SpriteRenderer feetRenderer2;
    public GameObject removedShoes;

    [Header("Arms Pivots")]
    public GameObject armsUpPivot;
    public GameObject armsDownPivot;

    private GameManager gameManager;
    private PeasantData peasantData;

    private bool isChoiceTime = false;
    private bool isChosen = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init(GameManager p_gameManager, PeasantData p_peasantData)
    {
        gameManager = p_gameManager;
        peasantData = p_peasantData;

        bodyRenderer.color = peasantData.baseColor;
        armUp1Renderer.color = peasantData.baseColor;
        armUp2Renderer.color = peasantData.baseColor;
        armDown1Renderer.color = peasantData.baseColor;
        armDown2Renderer.color = peasantData.baseColor;

        //patternRenderer.color = peasantData.patternColor;
        //armUp1PatternRenderer.color = peasantData.patternColor;
        //armUp2PatternRenderer.color = peasantData.patternColor;
        //armDown1PatternRenderer.color = peasantData.patternColor;
        //armDown2PatternRenderer.color = peasantData.patternColor;
        patternRenderer.color = peasantData.baseColor;
        armUp1PatternRenderer.color = peasantData.baseColor;
        armUp2PatternRenderer.color = peasantData.baseColor;
        armDown1PatternRenderer.color = peasantData.baseColor;
        armDown2PatternRenderer.color = peasantData.baseColor;

        patternRenderer.sprite = gameManager.peasantParts.patternMasks[peasantData.patternId];

        if (peasantData.isTarget)
        {
            feetRenderer1.sprite = gameManager.peasantParts.badFeetSprite;
            feetRenderer2.sprite = gameManager.peasantParts.badFeetSprite;
        }
        else
        {
            feetRenderer1.sprite = gameManager.peasantParts.goodFeetSprite;
            feetRenderer2.sprite = gameManager.peasantParts.goodFeetSprite;
        }

        removedShoes.SetActive(true);
    }

    public void SetExpression(PeasantExpression expression)
    {
        switch (expression)
        {
            case PeasantExpression.Chill:
                expressionRenderer.sprite = gameManager.peasantParts.chillExpressionSprite;
                break;
            case PeasantExpression.Surprise:
                expressionRenderer.sprite = gameManager.peasantParts.surpriseExpressionSprite;
                break;
            case PeasantExpression.Pain:
                expressionRenderer.sprite = gameManager.peasantParts.painExpressionSprite;
                break;
            case PeasantExpression.Death:
                expressionRenderer.sprite = gameManager.peasantParts.deathExpressionSprite;
                break;
        }
    }

    public void ShowFeet()
    {
        if (peasantData.isTarget)
        {
            feetRenderer1.sprite = gameManager.peasantParts.badFeetSprite;
            feetRenderer2.sprite = gameManager.peasantParts.badFeetSprite;
        }
        else
        {
            feetRenderer1.sprite = gameManager.peasantParts.goodFeetSprite;
            feetRenderer2.sprite = gameManager.peasantParts.goodFeetSprite;
        }
        removedShoes.SetActive(true);
    }
    public void HideFeet()
    {
        feetRenderer1.sprite = gameManager.peasantParts.shoesSprite;
        feetRenderer2.sprite = gameManager.peasantParts.shoesSprite;

        isChoiceTime = true;

        removedShoes.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isChoiceTime || isChosen)
            return;

        isChosen = true;

        SetExpression(PeasantExpression.Surprise);
        armsDownPivot.SetActive(false);
        armsUpPivot.SetActive(true);

        ShowFeet();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isChoiceTime || isChosen)
            return;

        SetExpression(PeasantExpression.Chill);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isChoiceTime || isChosen)
            return;

        SetExpression(PeasantExpression.Surprise);
    }
}
