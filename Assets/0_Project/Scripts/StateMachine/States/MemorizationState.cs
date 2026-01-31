using UnityEngine;

public class MemorizationState : IGameState
{
    private GameManager gameManager;
    private float timer = 0;
    private bool running = false;

    public MemorizationState(GameManager gm)
    {
        gameManager = gm;
    }

    public void Enter()
    {
        Debug.Log("Fase di memorizzazione");
        // Mostra elementi da memorizzare

        gameManager.SetupShuffle();

        timer = 0;

        running = true;
    }

    public void Update()
    {
        if (running)
        {
            timer += Time.deltaTime;

            if (PlayerReadyToChoose())
            {
                gameManager.ChangeState(gameManager.choiceState);
            }
        }
    }

    public void Exit()
    {
        Debug.Log("Fine memorizzazione");
    }

    private bool PlayerReadyToChoose()
    {
        return timer >= gameManager.memorizationTimer;
    }
}
