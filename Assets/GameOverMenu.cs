using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverWindow;

    private void OnEnable()
    {
        CountdownTimer.OnTimerEnd += EnableGameOverMenu;   
    }

    private void EnableGameOverMenu()
    {
        if (gameOverWindow != null)
        {
            gameOverWindow.SetActive(true);
        }

    }

    public void RestartScene()
    {
        SceneManager.LoadScene(1);
    }

/*    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }*/

}
