using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <attackSummary>
/// 
/// 0 = normal Attack
/// 1 = frenzy
/// 2 = warcry
/// 
/// </attackSummary>
/// 
/// <statesSummary>
/// 
/// 0 = idle
/// (1 = walk)--NOT
/// 1 = jump
/// 2 = attack
/// 3 = devour
/// 
/// </statesSummary>

[RequireComponent(typeof(Attack))]
[RequireComponent(typeof(PlayerControlsStateMachine))]

public class PlayerStateMachine : MonoBehaviour
{
    [HideInInspector] public PlayerState playerState;
    [HideInInspector] public bool facingRight = true;
    [HideInInspector] private float lastMove = 1;

    //playerValue
    [Title("Variabili base del player.")]
    public bool airControl = true;
    public int lifeIncrement = 25;
    public int lifeHealWhenEat = 20;
    public float dashSpeed = 500f;
    public float jumpForce = 20000f;
    public float moveForce = 10f;
    
    [Title("variabili del QTE per magnà.")]
    public bool abilitataClickMode = true;//se abilitata si clicca, altrimenti i dorsali vanno tenuti premuti
    [EnableIf("abilitataClickMode")]
    public bool abilitateTimeForQuickTimeEvent = true;
    [EnableIf("abilitataClickMode")]
    public int clickForEat = 4;
    [EnableIf("abilitataClickMode")]
    public float QTETime = 5f;
    [DisableIf("abilitataClickMode")]
    public float pressedTime = 5f;
    private float timeWasPressed = 0f;

    [Title("Audio del player.")]
    public AudioClip jump;
    public AudioClip walk;
    public AudioClip devour;

    private AudioSource eatAudio;
    private AudioSource jumpAudio;
    private AudioSource walkAudio;
    /*
    [Title("Cambiarle anche nell'input, non solo qui.")]
    public string LeftButtonQTE = "I";
    public string RightButtonQTE = "O";*/

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
    private bool QTEOnlyone = true;
    private bool QTEButtonAlreadyDown = false;
    //button 4 QuickTimeEvent
    //private bool QTEButtonLeft = false;
    //private bool QTEButtonRight = false;
    private float QTEButtonLeft;
    private float QTEButtonRight;
    private float QTEIsPressedForFloat = .5f;

    [Title("ReadOnly, modifiche disabilitate.")]
    [ReadOnly]
    public GameObject playerLife;
    [ReadOnly]
    public Collider2D eatCollider;
    [ReadOnly]
    public ContactFilter2D contactFilter;
    [ReadOnly]
    public SpriteRenderer offenseStateSpriteRenderer;
    //[ReadOnly]
    public GameObject textQTE;
    //[ReadOnly]
    public TextMesh textCountdownQTE;

    //private float 
    private float realGroundRadiusCollision = .1f;
    private float groundRadiusCollision = 0f;
    private int maxEnemyDeadHittedArray = 100;
    private bool waited = false;
    private bool singleJump = true;
    private bool activeFrenzy = false;

    //animatr thngs
    private bool exitAnimation = false;
    //

    [Title("Modifiche abilitate.")]
    [SerializeField] private bool isGrounded = true;
    private int i = 0; //counter
    private int eatCountdown = 0;//the eat countown support variable forchè yes
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
        timeWasPressed = pressedTime;
        anim = GetComponent<Animator>();
        life = GetComponent<Life>();
        rb2d = GetComponent<Rigidbody2D>();
        groundRadiusCollision = realGroundRadiusCollision;
        enemyDeadHitted = new Collider2D[maxEnemyDeadHittedArray];
        //textQTE.text = LeftButtonQTE.ToUpper() + "                    " + RightButtonQTE.ToUpper();
        EnableDisableQTEIcon(false);
        //textQTE.GetComponentInChildren<SpriteRenderer>().enabled = false;

        playerOffenseStateStandardColor = Color.white;
        playerOffenseStateAttackColor = Color.red;
        playerOffenseStateEatColor = Color.green;
        offenseStateSpriteRenderer.color = playerOffenseStateStandardColor;
    }

    // Use this for initialization
    private void Start ()
    {
        //instantiate all the audio
        eatAudio = gameObject.AddComponent<AudioSource>();
        eatAudio.clip = devour;
        eatAudio.playOnAwake = false;
        jumpAudio = gameObject.AddComponent<AudioSource>();
        jumpAudio.clip = jump;
        jumpAudio.playOnAwake = false;
        walkAudio = gameObject.AddComponent<AudioSource>();
        walkAudio.volume = .05f;
        walkAudio.clip = walk;
        walkAudio.playOnAwake = false;
        walkAudio.loop = true;

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

        if(playerState != PlayerState.movement)
        {
            //Debug.Log("_____________________________________");
            anim.SetFloat("Speed", 0);
        }
    }
    
    private void Update ()
    {
        //Debug.Log("CD" + eatCountdown);

        if(abilitataClickMode)
        {
            textCountdownQTE.text = eatCountdown.ToString();
        }
        else
        {
            textCountdownQTE.text = (Mathf.Round(timeWasPressed * 100) / 100).ToString();
        }
        
        //____________________________EAT________________________//
        if (CheckIfAnyoneDead())
        {
            EnableDisableQTEIcon(true);
            playerState = PlayerState.eat;
            //Eat();
        }
        else
        {
            timeWasPressed = pressedTime;
            eatCountdown = clickForEat;
            EnableDisableQTEIcon(false);
        }
        //________________________________________________________
        
        if (playerState == PlayerState.movement)
        {
            if(!walkAudio.isPlaying)
            {
                walkAudio.Play();
            }
        }
        else
        {
            
            if (walkAudio.isPlaying)
            {
                walkAudio.Pause();
            }
        }
        
        //Debug.Log(playerState);
        switch (playerState)
        {
            case PlayerState.idle:
                anim.SetInteger("States", 0);
                Idle();
                break;

            case PlayerState.attack:
                anim.SetInteger("States", 2);
                //StartCoroutine(Wait(.1f, false));
                //Attack(playerAttack);
                break;

            case PlayerState.movement:
                Movement(playerMovement);
                break;

            case PlayerState.jump:
                //anim.SetInteger("States", 1);
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
        
        if (isGrounded)
        {
            singleJump = true;
        }
        groundRadiusCollision = realGroundRadiusCollision;
        //Debug.Log(gameObject.layer);
        //TODO: animation start
    }

    public void Attack(int attackType)
    {
        attack.DoAttack(attackType, facingRight);

        //playerState = PlayerState.idle;
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
        anim.SetFloat("Speed", rb2d.velocity.magnitude);
        
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
            //playerState = PlayerState.idle;
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
            jumpAudio.Play();
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
        StartCoroutine(WaitForLayer(.1f));
        //TODO: wait 4 animation

    }

    private void Eat()
    {
        //if (CheckIfAnyoneDead())
        //{
        //textQTE.GetComponentInChildren<SpriteRenderer>().enabled = true;
        /*
        if (abilitateTimeForQuickTimeEvent && QTEOnlyone)
        {
            StartCoroutine(TimeForQuickTime());
        }*/
        //if (QTEButtonRight < -QTEIsPressedForFloat && QTEButtonLeft < QTEIsPressedForFloat && !QTEButtonAlreadyDown)

        if (abilitataClickMode)
        {
            //modalità a click
            if (QTEButtonRight < QTEIsPressedForFloat && QTEButtonLeft < QTEIsPressedForFloat && !QTEButtonAlreadyDown)
            {
                QTEButtonAlreadyDown = true;
                eatCountdown--;
                if (eatCountdown <= 0)
                {
                    eatCountdown = clickForEat;
                    //textQTE.GetComponentInChildren<SpriteRenderer>().enabled = false;
                    if (abilitateTimeForQuickTimeEvent && QTEOnlyone)
                    {
                        EatEnemy();
                        //StopCoroutine(TimeForQuickTime());
                        QTEOnlyone = true;
                    }
                    else
                    {
                        EatEnemy();
                    }
                }

            }

            if (QTEButtonAlreadyDown && QTEButtonRight > QTEIsPressedForFloat && QTEButtonLeft > QTEIsPressedForFloat)
            {
                QTEButtonAlreadyDown = false;
            }
            //}
            /*
            else
            {
                StopCoroutine(TimeForQuickTime());
                playerState = PlayerState.idle;
                EnableDisableQTEIcon(false);
                QTEOnlyone = true;
            }*/
        }
        else
        {
            //Debug.Log("right button: " + QTEButtonRight);
            //Debug.Log("left button: " + QTEButtonLeft);
            if (QTEButtonRight > QTEIsPressedForFloat && QTEButtonLeft > QTEIsPressedForFloat)
            {
                //playerState = PlayerState.eat;
                anim.SetInteger("States", 3);
                timeWasPressed -= Time.deltaTime;
                if (timeWasPressed <= 0)
                {
                    timeWasPressed = pressedTime;
                    playerState = PlayerState.idle;
                    anim.SetInteger("States", 0);
                    EatEnemy();
                }
            }
            else
            {
                timeWasPressed = pressedTime;
                playerState = PlayerState.idle;
                anim.SetInteger("States", 0);
            }
        }

        if(exitAnimation)
        {
            playerState = PlayerState.idle;
            anim.SetInteger("States", 0);
        }
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

    private bool CheckIfAnyoneDead()
    {
        for(int i=0; i< enemyDeadHitted.Length; i++)
        {
            enemyDeadHitted[i] = null;
        }

        eatCollider.OverlapCollider(contactFilter, enemyDeadHitted);
        i = 0;
        foreach (Collider2D collider in enemyDeadHitted)
        {
            if (enemyDeadHitted[i] != null)
            {
                if (enemyDeadHitted[i].CompareTag("Corpse"))
                {
                    return true;
                }
            }
            i++;
        }
        return false;
    }

    private void EatEnemy()
    {
        eatCollider.OverlapCollider(contactFilter, enemyDeadHitted);
        i = 0;
        foreach (Collider2D collider in enemyDeadHitted)
        {
            if (enemyDeadHitted[i] != null)
            {
                if (enemyDeadHitted[i].CompareTag("Corpse"))
                {
                    eatAudio.Play();
                    Destroy(enemyDeadHitted[i].gameObject);
                    IncrementLife(lifeIncrement);
                    Heal(lifeHealWhenEat);
                    //globalVariables.enemyDead++;
                    //Debug.Log(globalVariables.enemyDead);
                }
            }
            i++;
        }
        playerState = PlayerState.idle;
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

    private void QTEButtonLeftIsDown(float button)
    {
        QTEButtonLeft = button;
        //Debug.Log("LEFT " + button);
    }

    private void QTEButtonRightIsDown(float button)
    {
        QTEButtonRight = button;
        //Debug.Log("RIGHT " + button);
    }

    private void EnableDisableQTEIcon(bool enableDisable)
    {
        foreach (SpriteRenderer sr in textQTE.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.enabled = enableDisable;
        }
        foreach (MeshRenderer me in textQTE.GetComponentsInChildren<MeshRenderer>())
        {
            me.enabled = enableDisable;
        }
    }

    /****************************************/
    /*animator things*/

    public void AnimatorExitAnimation()
    {
        exitAnimation = true;
    }

    public void AnimatorGoToIdle()
    {
        playerState = PlayerState.idle;
        anim.SetInteger("States", 0);
    }

    /***************************************/

    IEnumerator TimeForQuickTime()
    {
        yield return new WaitForSeconds(QTETime);
        EnableDisableQTEIcon(false);
        //textQTE.SetActive(false);
        QTEOnlyone = true;
        playerState = PlayerState.idle;
    }

    IEnumerator WaitForLayer(float sec)
    {
        yield return new WaitForSeconds(sec);
        gameObject.layer = 8;
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
