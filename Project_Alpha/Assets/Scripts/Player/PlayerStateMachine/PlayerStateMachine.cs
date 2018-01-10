using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <attackSummary>
/// 
/// 0 = normal Attack
/// 
/// 
/// 
/// </attackSummary>

[RequireComponent(typeof(Attack))]
[RequireComponent(typeof(PlayerControlsStateMachine))]

public class PlayerStateMachine : MonoBehaviour
{
    [HideInInspector] public PlayerState playerState;
    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public float lastMove = 0;

    //playerValue
    public bool airControl = true;
    public int heal = 50;
    public float dashSpeed = 5f;
    private float jumpForce = 20f;
    public float moveForce = 10f;

    [HideInInspector] public float playerMovement;
    [HideInInspector] public int playerAttack;
    //[HideInInspector] public bool playerDash;
    //[HideInInspector] public bool playerEat;
    //[HideInInspector] public bool playerJump;

    //scripts
    private GlobalVariables globalVariables;
    private Attack attack;
    private Life life;
    private Movement movement;

    //other
    [SerializeField] private Transform groundCheck;
    private Animator anim;
    [SerializeField] private LayerMask whatIsGround;
    private Rigidbody2D rb2d;
    private Collider2D[] enemyDeadHitted;

    public Collider2D eatCollider;
    public ContactFilter2D contactFilter;

    private float groundRadiusCollision = .3f;
    private int maxEnemyDeadHittedArray = 100;
    private bool singleJump = true;
    [SerializeField] private bool isGrounded = false;
    private int i = 0; //counter
    

    public enum PlayerState
    {
        idle,
        attack,
        movement,
        jump,
        dash,
        eat
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        life = GetComponent<Life>();
        rb2d = GetComponent<Rigidbody2D>();
        enemyDeadHitted = new Collider2D[maxEnemyDeadHittedArray];
    }

    // Use this for initialization
    private void Start ()
    {
        globalVariables = GameObject.Find("GameManager").GetComponent<GlobalVariables>();
        attack = GetComponent<Attack>();
        movement = GetComponent<Movement>();
    }

    private void FixedUpdate()
    {
        isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundRadiusCollision, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
            }
        }
        anim.SetBool("Ground", isGrounded);
        // Set the vertical animation
        anim.SetFloat("vSpeed", rb2d.velocity.y);
    }

    // Update is called once per frame
    private void Update ()
    {
        Debug.Log(playerState);
        switch (playerState)
        {
            case PlayerState.idle:
                Idle();
                break;

            case PlayerState.attack:
                Attack(playerAttack);
                break;

            case PlayerState.movement:
                Movement(playerMovement);
                break;

            case PlayerState.jump:
                Jump(playerMovement);
                break;

            case PlayerState.dash:
                Dash();
                break;

            case PlayerState.eat:
                Eat();
                break;

            default:
                break;
        }
    }

    ///////////////////////////
    //
    //State machine Things
    //
    ///////////////////////////

    private void Idle()
    {
        singleJump = true;
        gameObject.layer = 8;
        //TODO: animation start
    }

    private void Attack(int attackType)
    {
        attack.DoAttack(attackType);

        playerState = PlayerState.idle;
    }

    private void Movement(float leftRightMove, bool jumpCall = true)
    {
        if(jumpCall)
        {
            //TODO: animation start
        }

        rb2d.velocity = new Vector2(leftRightMove * moveForce, rb2d.velocity.y);

        lastMove = leftRightMove;

        //Flip The player to watch in the right way
        if (leftRightMove < 0 && facingRight)
        {
            Flip();
        }
        else if (leftRightMove > 0 && !facingRight)
        {
            Flip();
        }

        if(jumpCall)
        {
            playerState = PlayerState.idle;
        }
    }

    private void Jump(float leftRightMove)
    {
        if(airControl)
        {
            Movement(leftRightMove, false);
        }

        if(singleJump)
        {
            rb2d.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            singleJump = false;
        }

        if (isGrounded)
        {
            playerState = PlayerState.idle;
        }
        //TODO: animation start
    }

    private void Dash()
    {
        gameObject.layer = 13;
        //TODO: wait 4 animation
        rb2d.MovePosition(rb2d.position + new Vector2(Mathf.Sign(lastMove) * dashSpeed, 0));
        //TODO: wait 4 animation
        
        playerState = PlayerState.idle;
    }

    private void Eat()
    {
        eatCollider.OverlapCollider(contactFilter, enemyDeadHitted);
        i = 0;
        foreach (Collider2D collider in enemyDeadHitted)
        {
            if (enemyDeadHitted[i] != null)
            {
                if (enemyDeadHitted[i].CompareTag("Corpse"))
                {
                    Destroy(enemyDeadHitted[i].gameObject);
                    life.Heal(heal);
                    globalVariables.enemyDead++;
                    //Debug.Log(globalVariables.enemyDead);
                }
            }

            i++;
        }

        playerState = PlayerState.idle;
    }


    ///////////////////////////
    //
    //Non state machine Things
    //
    ///////////////////////////

    private void Flip()
    {
        //Debug.Log("Do Flip");
        facingRight = !facingRight;
        Vector3 normalScale = transform.localScale;
        normalScale.x *= -1;
        transform.localScale = normalScale;
    }
}
