using UnityEngine;

public class CutsceneState : IGameState
{
    private GameManager gameManager;
    private float timer = 0;
    private bool running = false;

    public CutsceneState(GameManager gm)
    {
        gameManager = gm;
    }

    public void Enter()
    {
        Debug.Log("Cutscene iniziata");
        // Avvia animazioni / timeline / video

        timer = 0;
        gameManager.cutscenePlaceholder.SetActive(true);

        running = true;
    }

    public void Update()
    {
        if (running)
        {
            timer += Time.deltaTime;

            // Quando la cutscene finisce
            if (CutsceneIsOver())
            {
                gameManager.ChangeState(gameManager.memorizationState);
            }
        }
    }

    public void Exit()
    {
        Debug.Log("Cutscene finita");
        gameManager.cutscenePlaceholder.SetActive(false);
    }

    private bool CutsceneIsOver()
    {
        return timer >= gameManager.cutsceneTimer;
    }
}
