using UnityEngine;

public class MemorizationState : IGameState
{
    private GameManager gameManager;

    public MemorizationState(GameManager gm)
    {
        gameManager = gm;
    }

    public void Enter()
    {
        Debug.Log("Fase di memorizzazione");
        // Mostra elementi da memorizzare
    }

    public void Update()
    {
        if (PlayerReadyToChoose())
        {
            gameManager.ChangeState(gameManager.choiceState);
        }
    }

    public void Exit()
    {
        Debug.Log("Fine memorizzazione");
    }

    private bool PlayerReadyToChoose()
    {
        return false;
    }
}
