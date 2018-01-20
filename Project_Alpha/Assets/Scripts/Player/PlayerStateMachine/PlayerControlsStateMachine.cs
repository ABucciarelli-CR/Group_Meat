using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlsStateMachine : MonoBehaviour
{
    private PlayerStateMachine stateMachine;

    private float leftAndRightMovement;
    private bool jump = false;
    private bool inputDash = false;
    private float axesDash = 0f;
    private bool eat = false;
    private bool canDash = true;

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
        inputDash = Input.GetButtonDown("DashInput");
        axesDash = Input.GetAxis("DashAxes");
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
            Jump();
            CheckAllTheAttack();
            Dash();
            
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
        if(inputDash)
        {
            stateMachine.playerState = PlayerStateMachine.PlayerState.dash;
        }
        
        if(axesDash == 1 && canDash)
        {
            canDash = false;
            stateMachine.playerState = PlayerStateMachine.PlayerState.dash;
        }

        if(axesDash <= 0.2f)
        {
            canDash = true;
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
