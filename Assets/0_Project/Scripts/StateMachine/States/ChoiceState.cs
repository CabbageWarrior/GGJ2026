using UnityEngine;

public class ChoiceState : IGameState
{
    private GameManager gameManager;

    public ChoiceState(GameManager gm)
    {
        gameManager = gm;
    }

    public void Enter()
    {
        Debug.Log("Fase di scelta");
        // Mostra UI di scelta

        gameManager.currentShuffle.StartShuffle();
    }

    public void Update()
    {
        if (ChoiceCompleted())
        {
            gameManager.ChangeState(gameManager.cutsceneState);
            // oppure vai avanti col livello
        }
    }

    public void Exit()
    {
        Debug.Log("Fine scelta");
    }

    private bool ChoiceCompleted()
    {
        return false;
    }
}
