using UnityEngine;

public class Peasant : MonoBehaviour
{
    public SpriteRenderer expressionRenderer;
    public SpriteRenderer bodyRenderer;
    public SpriteRenderer patternRenderer;
    public SpriteMask patternMask;
    public SpriteRenderer feetRenderer;
    public GameObject removedShoes;

    private GameManager gameManager;
    private PeasantData peasantData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init(GameManager p_gameManager, PeasantData p_peasantData)
    {
        gameManager = p_gameManager;
        peasantData = p_peasantData;

        bodyRenderer.color = peasantData.baseColor;
        patternRenderer.color = peasantData.patternColor;
        patternMask.sprite = gameManager.peasantParts.patternMasks[peasantData.patternId];
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
            feetRenderer.sprite = gameManager.peasantParts.badFeetSprite;
        else
            feetRenderer.sprite = gameManager.peasantParts.goodFeetSprite;

        removedShoes.SetActive(true);
    }
    public void HideFeet()
    {
        feetRenderer.sprite = gameManager.peasantParts.shoesSprite;

        removedShoes.SetActive(false);
    }
}
