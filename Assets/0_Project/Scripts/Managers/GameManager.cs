using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public PeasantPartsScriptable peasantParts;

    private GameStateMachine stateMachine;

    // Game states
    public TutorialState tutorialState;
    public CutsceneState cutsceneState;
    public MemorizationState memorizationState;
    public ChoiceState choiceState;

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
}
