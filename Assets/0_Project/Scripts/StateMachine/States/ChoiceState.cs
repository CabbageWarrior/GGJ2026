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

        gameManager.currentShuffle.SetChoicePhase(true);
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
        gameManager.currentShuffle.SetChoicePhase(false);

        Debug.Log("Fine scelta");
    }

    private bool ChoiceCompleted()
    {
        return false;
    }
}
