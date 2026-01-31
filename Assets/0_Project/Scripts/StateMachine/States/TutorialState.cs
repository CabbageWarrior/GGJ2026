using UnityEngine;

public class TutorialState : IGameState
{
    private GameManager gameManager;
    private float timer = 0;
    private bool running = false;

    public TutorialState(GameManager gm)
    {
        gameManager = gm;
    }

    public void Enter()
    {
        Debug.Log("Tutorial iniziato");

        timer = 0;
        gameManager.tutorialPlaceholder.SetActive(true);

        running = true;
    }

    public void Update()
    {
        if (running)
        {
            timer += Time.deltaTime;

            if (timer >= gameManager.tutorialTimer)
            {
                EndTutorial();
            }
        }
    }

    public void Exit()
    {
        Debug.Log("Tutorial finito");
        gameManager.tutorialPlaceholder.SetActive(false);
    }

    private void ShowStep(int step)
    {
        Debug.Log("Tutorial step: " + step);
        // Mostra testo / highlight / frecce / UI
    }

    private void EndTutorial()
    {
        running = false;

        gameManager.ChangeState(gameManager.cutsceneState);
    }
}
