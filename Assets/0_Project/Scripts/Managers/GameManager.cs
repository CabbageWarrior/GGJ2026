using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

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

    [Header("==> End")]
    public GameObject gameEndGO;
    public GameObject gameEndGoodGO;
    public GameObject gameEndBadGO;
    #endregion

    private GameStateMachine stateMachine;

    // Game states
    public TutorialState tutorialState;
    public MemorizationState memorizationState;
    public ChoiceState choiceState;
    public GameEndState gameEndState;

    [HideInInspector]
    public PeopleShuffle currentShuffle;

    [HideInInspector]
    public bool isWin = false;

    [HideInInspector]
    public int tentativCounter = 0;
    [HideInInspector]
    public int errorCounter = 0;

    void Awake()
    {
        stateMachine = new GameStateMachine();

        tutorialState = new TutorialState(this);
        memorizationState = new MemorizationState(this);
        choiceState = new ChoiceState(this);
        gameEndState = new GameEndState(this);
    }

    void Start()
    {
        ChangeState(tutorialState);
    }

    void Update()
    {
        stateMachine.Update();
    }
    void ResetGame()
    {
        isWin = false;
        tentativCounter = 0;
        errorCounter = 0;

        ChangeState(tutorialState);
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
        isWin = true;
        ChangeState(gameEndState);
    }
    private void DoGameOver()
    {
        Debug.Log("Game Over!");
        isWin = false;
        ChangeState(gameEndState);
    }
}
