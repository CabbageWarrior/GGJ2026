using UnityEngine;

public class TutorialState : IGameState
{
    private GameManager gameManager;
    private int currentStep = 0;

    public TutorialState(GameManager gm)
    {
        gameManager = gm;
    }

    public void Enter()
    {
        Debug.Log("Tutorial iniziato");
        currentStep = 0;
        ShowStep(currentStep);
    }

    public void Update()
    {
        if (StepCompleted())
        {
            currentStep++;

            if (currentStep >= GetTotalSteps())
            {
                EndTutorial();
            }
            else
            {
                ShowStep(currentStep);
            }
        }
    }

    public void Exit()
    {
        Debug.Log("Tutorial finito");
        HideTutorialUI();
    }

    private void ShowStep(int step)
    {
        Debug.Log("Tutorial step: " + step);
        // Mostra testo / highlight / frecce / UI
    }

    private bool StepCompleted()
    {
        // input giocatore o evento (click, movimento, ecc.)
        return false;
    }

    private int GetTotalSteps()
    {
        return 3; // esempio
    }

    private void EndTutorial()
    {
        gameManager.ChangeState(gameManager.cutsceneState);
    }

    private void HideTutorialUI()
    {
        // nascondi overlay tutorial
    }
}
