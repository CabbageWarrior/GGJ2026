using UnityEngine;

public class Peasant : MonoBehaviour
{
    public SpriteRenderer expressionRenderer;
    public SpriteRenderer bodyRenderer;
    public SpriteRenderer patternRenderer;
    public SpriteMask patternMask;
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

    public void SetExpression(Sprite p_expressionSprite)
    {
        expressionRenderer.sprite = p_expressionSprite;
    }
}
