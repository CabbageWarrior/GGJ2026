using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Inspector info
    [Header("References")]
    public PeasantPartsScriptable peasantParts;

    [Header("States Params", order = 0)]
    [Header("==> Tutorial", order = 1)]
    public GameObject tutorialPlaceholder;
    public float tutorialTimer = 2f;

    [Header("==> Cutscene")]
    public GameObject cutscenePlaceholder;
    public float cutsceneTimer = 3f;

    [Header("==> GamePlay")]
    public float memorizationTimer = 10f;
    public PeopleShuffle[] peopleShuffles;
    public GameObject peasantPrefab;
    #endregion

    private GameStateMachine stateMachine;

    // Game states
    public TutorialState tutorialState;
    public CutsceneState cutsceneState;
    public MemorizationState memorizationState;
    public ChoiceState choiceState;

    [HideInInspector]
    public PeopleShuffle currentShuffle;

    private int tentativCounter = 0;
    private int errorCounter = 0;

    void Awake()
    {
        stateMachine = new GameStateMachine();

        tutorialState = new TutorialState(this);
        cutsceneState = new CutsceneState(this);
        memorizationState = new MemorizationState(this);
        choiceState = new ChoiceState(this);
    }

    void Start()
    {
        stateMachine.ChangeState(tutorialState);
    }

    void Update()
    {
        stateMachine.Update();
    }

    public void ChangeState(IGameState newState)
    {
        stateMachine.ChangeState(newState);
    }

    public void SetupShuffle()
    {
        if (currentShuffle)
        {
            currentShuffle.gameObject.SetActive(false);
        }

        int shuffleIndex = Random.Range(0, peopleShuffles.Length);
        currentShuffle = peopleShuffles[shuffleIndex];
        currentShuffle.gameObject.SetActive(true);
        currentShuffle.SetupShuffle(peasantPrefab, peasantParts);
    }
    public void StartShuffle()
    {
        currentShuffle.StartShuffle();
    }

    public void AddTentativ()
    {
        tentativCounter++;

        if (tentativCounter - errorCounter == currentShuffle.badPeople)
        {
            DoWin();
        }
    }
    public void AddError()
    {
        errorCounter++;

        if (errorCounter >= 3)
        {
            DoGameOver();
        }
    }

    private void DoWin()
    {
        Debug.Log("You Win!");
    }
    private void DoGameOver()
    {
        Debug.Log("Game Over!");
    }
}
