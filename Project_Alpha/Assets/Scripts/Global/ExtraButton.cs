using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManagerAction))]
public class ExtraButton : MonoBehaviour
{
    private GameManagerAction gameManager;

    private bool pause = false;

    // Use this for initialization
    void Start ()
    {
        gameManager = GetComponent<GameManagerAction>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        gameManager.RestartTheLevel(Input.GetButtonDown("Restart"));
        if(!pause && Input.GetButtonDown("Pause"))
        {
            gameManager.PauseLevel(Input.GetButtonDown("Pause"));
            pause = true;
        }
        else if(pause && Input.GetButtonDown("Pause"))
        {
            gameManager.ExitPauseLevel(Input.GetButtonDown("Pause"));
            pause = false;
        }
    }
}
