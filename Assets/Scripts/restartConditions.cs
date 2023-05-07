using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class restartConditions : MonoBehaviour
{
    // Screens to activate
    public Canvas winScreen, loseScreen;
    // Lose conditions
    public boundary mainWall, leftWall, rightWall;
    private float mainWallHealth, leftWallHealth, rightWallHealth;

    // Keep getting updated health
    void Update()
    {
        mainWallHealth = mainWall.health;
        leftWallHealth = leftWall.health;
        rightWallHealth = rightWall.health;

        if (mainWallHealth <= 0 || leftWallHealth <= 0 || rightWallHealth <= 0)
        {
            youLose();
        }
    }

    void youLose()
    {
        loseScreen.gameObject.SetActive(true);
    }

    public void youWin()
    {
        winScreen.gameObject.SetActive(true);
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}