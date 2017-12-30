using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerAction : MonoBehaviour
{
    private GlobalVariables globalVariables;

    private void Awake()
    {
        globalVariables = GetComponent<GlobalVariables>();
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
}
