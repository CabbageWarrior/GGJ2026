using UnityEngine;

public class GameEndState : IGameState
{
    private GameManager gameManager;

    public GameEndState(GameManager gm)
    {
        gameManager = gm;
    }

    public void Enter()
    {
        Debug.Log("Fase di fine");
        // Mostra UI di scelta

        if (gameManager.gameEndGO) gameManager.gameEndGO?.SetActive(true);
        if (gameManager.gameEndGoodGO) gameManager.gameEndGoodGO?.SetActive(gameManager.isWin);
        if (gameManager.gameEndBadGO) gameManager.gameEndBadGO?.SetActive(!gameManager.isWin);
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
        if (gameManager.gameEndGO) gameManager.gameEndGO?.SetActive(false);
        if (gameManager.gameEndGoodGO) gameManager.gameEndGoodGO?.SetActive(false);
        if (gameManager.gameEndBadGO) gameManager.gameEndBadGO?.SetActive(false);

        gameManager.currentShuffle.SetChoicePhase(false);

        Debug.Log("Fine scelta");
    }

    private bool ChoiceCompleted()
    {
        return false;
    }
}
