﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GameManagerAction))]
public class ExtraButton : MonoBehaviour
{
    private GameManagerAction gameManager;

    private bool pause = false;
    private int actualLevel;

    // Use this for initialization
    void Start ()
    {
        actualLevel = SceneManager.GetActiveScene().buildIndex;
        gameManager = GetComponent<GameManagerAction>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (actualLevel != SceneManager.GetActiveScene().buildIndex)
        {
            Debug.Log("changingScene");
            actualLevel = SceneManager.GetActiveScene().buildIndex;
            pause = false;
        }

        gameManager.RestartTheLevel(Input.GetButtonDown("Restart"));

        if(!pause && Input.GetButtonDown("Pause"))
        {
            Debug.Log("01");
            gameManager.PauseLevel(Input.GetButtonDown("Pause"));
            pause = true;
        }
        else if(pause && Input.GetButtonDown("Pause"))
        {
            Debug.Log("02");
            gameManager.ExitPauseLevel(Input.GetButtonDown("Pause"));
            pause = false;
        }
    }
}