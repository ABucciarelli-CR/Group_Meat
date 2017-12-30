using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManagerAction))]
public class ExtraButton : MonoBehaviour
{
    private bool restartLevel = false;
    private GameManagerAction gameManager;

    // Use this for initialization
    void Start ()
    {
        gameManager = GetComponent<GameManagerAction>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        restartLevel = Input.GetButtonDown("Restart");
        gameManager.RestartTheLevel(restartLevel);
        
    }
}
