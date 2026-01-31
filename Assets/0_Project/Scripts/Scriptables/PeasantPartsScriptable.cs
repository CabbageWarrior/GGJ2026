using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PeasantPartsScriptable", menuName = "JamProject/PeasantParts", order = 1)]
public class PeasantPartsScriptable : ScriptableObject
{
    [Header("Body Parts")]
    public List<Sprite> patternMasks;
    public List<Color> baseColors;
    public List<Color> patternColors;
    public Sprite goodFeetSprite;
    public Sprite badFeetSprite;
    public Sprite shoesSprite;

    [Header("Expressions")]
    public Sprite chillExpressionSprite;
    public Sprite surpriseExpressionSprite;
    public Sprite painExpressionSprite;
    public Sprite deathExpressionSprite;
}
