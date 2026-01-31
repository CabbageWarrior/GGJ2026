public class GameStateMachine
{
    private IGameState currentState;

    public void ChangeState(IGameState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        if (currentState != null)
            currentState.Update();
    }
}
