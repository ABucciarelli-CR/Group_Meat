using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerAction : MonoBehaviour
{
    
    public void RestartTheLevel(bool isRestart)
    {
        if (isRestart)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
