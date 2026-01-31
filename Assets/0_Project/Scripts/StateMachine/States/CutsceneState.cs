using UnityEngine;

public class CutsceneState : IGameState
{
    private GameManager gameManager;

    public CutsceneState(GameManager gm)
    {
        gameManager = gm;
    }

    public void Enter()
    {
        Debug.Log("Cutscene iniziata");
        // Avvia animazioni / timeline / video
    }

    public void Update()
    {
        // Quando la cutscene finisce
        if (CutsceneIsOver())
        {
            gameManager.ChangeState(gameManager.memorizationState);
        }
    }

    public void Exit()
    {
        Debug.Log("Cutscene finita");
    }

    private bool CutsceneIsOver()
    {
        // logica tua (timer, Timeline, evento, ecc.)
        return false;
    }
}
