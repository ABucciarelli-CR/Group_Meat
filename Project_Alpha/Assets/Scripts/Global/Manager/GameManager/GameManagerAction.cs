using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerAction : MonoBehaviour
{
    public GameObject canvas;

    private GlobalVariables globalVariables;
    private int actualLevel;

    private void Awake()
    {
        actualLevel = SceneManager.GetActiveScene().buildIndex;
        globalVariables = GetComponent<GlobalVariables>();
        canvas.SetActive(false);
    }

    private void Update()
    {
        if(actualLevel != SceneManager.GetActiveScene().buildIndex)
        {
            canvas.SetActive(false);
            Time.timeScale = 1;
            actualLevel = SceneManager.GetActiveScene().buildIndex;
        }
    }

    public void RestartTheLevel(bool isRestart)
    {
        if (isRestart)
        {
            globalVariables.closeDoor = false;
            globalVariables.enemyDead = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void PauseLevel(bool pause)
    {
        if(pause && SceneManager.GetActiveScene().buildIndex != 0)
        {
            Time.timeScale = 0;
            canvas.SetActive(true);
        }
    }

    public void ExitPauseLevel(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 1;
            canvas.SetActive(false);
        }
    }
}
