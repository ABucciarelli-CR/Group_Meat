using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <attackSummary>
/// 
/// 0 = normal Attack
/// 1 = frenzy
/// 2 = warcry
/// 
/// </attackSummary>

[RequireComponent(typeof(Attack))]
[RequireComponent(typeof(PlayerControlsStateMachine))]

public class PlayerStateMachine : MonoBehaviour
{
    [HideInInspector] public PlayerState playerState;
    [HideInInspector] public bool facingRight = true;
    [HideInInspector] private float lastMove = 1;

    //playerValue
    public bool airControl = true;
    public int lifeIncrement = 25;
    public float dashSpeed = 500f;
    public float jumpForce = 20f;
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

    //other
    [SerializeField] private Transform groundCheck;
    private Animator anim;
    [SerializeField] private LayerMask whatIsGround;
    private Rigidbody2D rb2d;
    private Collider2D[] enemyDeadHitted;

    [Title("ReadOnly, modifiche disabilitate.")]
    [ReadOnly]
    public GameObject playerLife;
    [ReadOnly]
    public Collider2D eatCollider;
    [ReadOnly]
    public ContactFilter2D contactFilter;
    [ReadOnly]
    public SpriteRenderer offenseStateSpriteRenderer;

    private float realGroundRadiusCollision = .1f;
    private float groundRadiusCollision = 0f;
    private int maxEnemyDeadHittedArray = 100;
    private bool waited = false;
    private bool singleJump = true;
    [Title("Modifiche abilitate.")]
    [SerializeField] private bool isGrounded = true;
    private int i = 0; //counter
    private Color playerOffenseStateStandardColor;
    private Color playerOffenseStateAttackColor;
    private Color playerOffenseStateEatColor;


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
        groundRadiusCollision = realGroundRadiusCollision;
        enemyDeadHitted = new Collider2D[maxEnemyDeadHittedArray];

        playerOffenseStateStandardColor = Color.white;
        playerOffenseStateAttackColor = Color.red;
        playerOffenseStateEatColor = Color.green;
        offenseStateSpriteRenderer.color = playerOffenseStateStandardColor;
    }

    // Use this for initialization
    private void Start ()
    {
        globalVariables = GameObject.Find("GameManager").GetComponent<GlobalVariables>();
        attack = GetComponent<Attack>();
    }

    private void FixedUpdate()
    {
        /*
        isGrounded = false;

        if (waited)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundRadiusCollision, whatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    isGrounded = true;
                }
            }
        }*/
        
        /*
        anim.SetBool("Ground", isGrounded);
        // Set the vertical animation
        anim.SetFloat("vSpeed", rb2d.velocity.y);*/
    }

    // Update is called once per frame
    private void Update ()
    {
        //Debug.Log(playerState);
        switch (playerState)
        {
            case PlayerState.idle:
                Idle();
                break;

            case PlayerState.attack:
                offenseStateSpriteRenderer.color = playerOffenseStateAttackColor;
                StartCoroutine(Wait(.1f, false));
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
                offenseStateSpriteRenderer.color = playerOffenseStateEatColor;
                StartCoroutine(Wait(.1f, false));
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
        if (isGrounded)
        {
            singleJump = true;
        }

        groundRadiusCollision = realGroundRadiusCollision;
        gameObject.layer = 8;
        //Debug.Log(gameObject.layer);
        //TODO: animation start
    }

    private void Attack(int attackType)
    {
        attack.DoAttack(attackType);

        playerState = PlayerState.idle;
    }

    private void Movement(float leftRightMove, bool jumpCall = true)
    {
        if (jumpCall)
        {
            if (isGrounded)
            {
                singleJump = true;
            }
            //TODO: animation start
        }
    

        rb2d.velocity = new Vector2(leftRightMove * moveForce, rb2d.velocity.y);
        
        //lastMove = leftRightMove;

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
            waited = false;
            StartCoroutine(Wait(.1f));
            isGrounded = false;
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
        //lastMove = leftRightMove;
        //TODO: wait 4 animation

        rb2d.velocity = new Vector2(0, 0);
        //Debug.Log(Mathf.Sign(lastMove));
        rb2d.MovePosition(rb2d.position + new Vector2(Mathf.Sign(lastMove) * dashSpeed, 0));
        playerState = PlayerState.idle;
        //TODO: wait 4 animation
        
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
                    IncrementLife(lifeIncrement);
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
        lastMove *= -1;
        Vector3 normalScale = transform.localScale;
        normalScale.x *= -1;
        transform.localScale = normalScale;
    }

    public void Damage(int dmg)
    {
        playerLife.GetComponent<Life>().Damage(dmg);
    }

    public void Heal(int heal)
    {
        playerLife.GetComponent<Life>().Heal(heal);
    }

    public void IncrementLife(int increment)
    {
        playerLife.GetComponent<Life>().IncrementLife(increment);
    }

    private void GoJump(bool isJump)
    {
        isGrounded = isJump;
    }

    IEnumerator Wait(float sec, bool returnWait = true)
    {
        yield return new WaitForSeconds(sec);
        if(returnWait)
        {
            waited = true;
        }
        else
        {
            offenseStateSpriteRenderer.color = playerOffenseStateStandardColor;
        }
        
    }
}
