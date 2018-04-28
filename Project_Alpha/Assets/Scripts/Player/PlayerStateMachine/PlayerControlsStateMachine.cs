using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerControlsStateMachine : MonoBehaviour
{
    /*States:
     * 
     * 0: IDLE
     * 1: JUMP
     * 2: ATTACK
     * 3: DASH
     * 
     */

    [ReadOnly]
    public DashCount dashCount;
    [ReadOnly]
    public Life life;
    [ReadOnly]
    public Animator animator;

    [Title("in percentuale, la vita che vienet tolta al player se non ha dash disponibili")]
    public float dashHealthConsume = 5f;
    public bool canDashRemovingLife = true;

    private PlayerStateMachine stateMachine;

    private float leftAndRightMovement;
    private bool jump = false;
    private bool inputDash = false;
    private float axesDash = 0f;
    //private bool eat = false;
    //private bool leftQTE = false;//left button 4 quick time event
    //private bool rightQTE = false;//right button 4 quick time event
    private float leftQTE;//left button 4 quick time event
    private float rightQTE;//right button 4 quick time event
    private bool canDash = true;
    private bool havePossibleDash = true;

    //attack Type
    //see summary in PlayerStateMachine.cs
    private int  normalAttack = 0;
    private int frenzyState = 1;
    private int warcry = 2;

    // Use this for initialization
    void Start ()
    {
        animator = GetComponent<Animator>();
        stateMachine = GetComponent<PlayerStateMachine>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        leftAndRightMovement = Input.GetAxis("Horizontal");
        inputDash = Input.GetButtonDown("DashInput");
        axesDash = Input.GetAxis("DashAxes");
        //eat = Input.GetButtonDown("Eat");
        jump = Input.GetButtonDown("Jump");

        //leftQTE = Input.GetButton("LeftEatQTE");
        //rightQTE = Input.GetButton("RightEatQTE");
        leftQTE = Input.GetAxis("LeftEatQTE");
        rightQTE = Input.GetAxis("RightEatQTE");

        stateMachine.SendMessage("QTEButtonLeftIsDown", leftQTE);
        stateMachine.SendMessage("QTEButtonRightIsDown", rightQTE);

        //print(inputDash);

        //se in Idle o in Movement
        if (stateMachine.playerState == PlayerStateMachine.PlayerState.idle)
        {
            Move();
            CheckAllTheAttack();
            Jump();
            Dash();
            //Eat();

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
            //Eat();

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

        if (Input.GetButtonDown("Frenzy"))
        {
            stateMachine.playerAttack = frenzyState;
            stateMachine.playerState = PlayerStateMachine.PlayerState.attack;
        }

        if (Input.GetButtonDown("Warcry"))
        {
            stateMachine.playerAttack = warcry;
            stateMachine.playerState = PlayerStateMachine.PlayerState.attack;
        }
    }

    private void Move()
    {
        animator.SetFloat("Speed", gameObject.GetComponent<Rigidbody2D>().velocity.magnitude);
        if (leftAndRightMovement != 0)
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
        //4 input
        if(inputDash && havePossibleDash)
        {
            stateMachine.playerState = PlayerStateMachine.PlayerState.dash;
            dashCount.SendMessage("RemoveOneCharge");
        }
        
        //4 gamepad
        if(axesDash == 1 && canDash && havePossibleDash)
        {
            dashCount.SendMessage("RemoveOneCharge");
            canDash = false;
            stateMachine.playerState = PlayerStateMachine.PlayerState.dash;
        }

        if(((axesDash == 1 && canDash) || inputDash) && !havePossibleDash && canDashRemovingLife)
        {
            //life.SendMessage("Damage", (int)((life.actualLife / 100) * dashHealthConsume));
            stateMachine.playerState = PlayerStateMachine.PlayerState.dash;
            life.actualLife -= (int)((life.actualLife / 100) * dashHealthConsume);
            if((int)((life.actualLife / 100) * dashHealthConsume) <= 0)
            {
                life.actualLife -= 1;
            }
        }
        /*if(axesDash <= 0.2f)
        {
            canDash = true;
        }*/
    }
    
    /*
    private void Eat()
    {
        if(eat)
        {
            //stateMachine.playerState = PlayerStateMachine.PlayerState.eat;
        }
    }*/

    private void HaveSomeDashLeft(bool boolDash)
    {
        havePossibleDash = boolDash;
    }
}
