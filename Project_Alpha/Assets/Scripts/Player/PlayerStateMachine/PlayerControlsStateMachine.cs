using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlsStateMachine : MonoBehaviour
{
    private PlayerStateMachine stateMachine;

    private float leftAndRightMovement;
    private bool jump = false;
    private bool dash = false;
    private bool eat = false;

    //attack Type
    //see summary in PlayerStateMachine.cs
    private int  normalAttack = 0;

    // Use this for initialization
    void Start ()
    {
        stateMachine = GetComponent<PlayerStateMachine>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        leftAndRightMovement = Input.GetAxis("Horizontal");
        dash = Input.GetButtonDown("Dash");
        eat = Input.GetButtonDown("Eat");
        jump = Input.GetButtonDown("Jump");

        //se in Idle o in Movement
        if(stateMachine.playerState == PlayerStateMachine.PlayerState.idle)
        {
            Move();
            CheckAllTheAttack();
            Jump();
            Dash();
            Eat();

            jump = false;
        }
        //se in Jump
        else if(stateMachine.playerState == PlayerStateMachine.PlayerState.jump)
        {
            Dash();
            Jump();

            jump = false;
        }
        //se in Movement
        else if(stateMachine.playerState == PlayerStateMachine.PlayerState.movement)
        {
            CheckAllTheAttack();
            Jump();
            Dash();
            Eat();

            jump = false;
        }

        stateMachine.playerMovement = leftAndRightMovement;
    }

    private void CheckAllTheAttack()
    {
        if(Input.GetButtonDown("Attack"))
        {
            stateMachine.playerAttack = normalAttack;
            stateMachine.playerState = PlayerStateMachine.PlayerState.attack;
        }
    }

    private void Move()
    {
        if(leftAndRightMovement != 0)
        {
            stateMachine.playerState = PlayerStateMachine.PlayerState.movement;
        }
    }

    private void Jump()
    {
        if(jump)
        {
            stateMachine.playerState = PlayerStateMachine.PlayerState.jump;
        }
    }

    private void Dash()
    {
        if(dash)
        {
            stateMachine.playerState = PlayerStateMachine.PlayerState.dash;
        }
    }

    private void Eat()
    {
        if(eat)
        {
            stateMachine.playerState = PlayerStateMachine.PlayerState.eat;
        }
    }
}
