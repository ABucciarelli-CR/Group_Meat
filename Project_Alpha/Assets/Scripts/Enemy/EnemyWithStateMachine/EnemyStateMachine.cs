using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 *STATES:
 * 
 * 0:IDLE
 * 1:ATTACK
 * 2:HEAL
 * 3:STUN
 * 
*/
[RequireComponent(typeof(Blink))]
public class EnemyStateMachine : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public EnemyState enemyState;
    /*[HideInInspector]*/
    public GameObject stun;


    [Title("ReadOnly, modifiche disabilitate.")]
    [ReadOnly]
    public float speed = .8f;
    [ReadOnly]
    public int damage;
    [ReadOnly]
    public float attackDelay;
    [ReadOnly]
    public float healDelay;

    public int TimeCanDeath = 2;
    [HideInInspector] public Vector2 movement;
    [HideInInspector] public float direction = 1;
    [HideInInspector] public float timeToChangeDirection = 5f;
    [HideInInspector] public float ttcd;
    [HideInInspector] public bool doPlayerDamage = false;
    //[ReadOnly]
    public bool facingRight = false;
    public bool dontResetFlipCountdown = false;
    [HideInInspector]public bool animationEnded = false;
    [HideInInspector]public bool arrivedToThePoint = false;
    /*[HideInInspector]*/

    [ReadOnly]
    public float stunTime = 5f;

    private float timeBeforeStoppedCoroutine = 0f;
    [ReadOnly]
    public float waitTimeBeforeFlip = .1f;
    [ReadOnly]
    public int healthRegenAfterStun = 20;//is in %
    [ReadOnly]
    public bool activeStunTime = true;
    [HideInInspector] public float delay;
    [HideInInspector] public int maxArray = 100;
    [HideInInspector] private bool stardCountdown = false;
    [HideInInspector] public bool regenerate = false;//to abilitate the enemy regeneration afrer stun
    [HideInInspector] public bool onlyOneDeath = true; //ceck to not die several times
    [HideInInspector] public bool waited = false;
    [HideInInspector] public bool alreadyInAttack = false;

    [HideInInspector] public EnemyHealth enemyHealth;
    [HideInInspector] public GameObject gameManager;
    [HideInInspector] public float stunnedTime = 0;

    [ReadOnly]
    public GameObject offenseState;//the gameObject that visualize

    [ReadOnly]
    public GameObject player;

    [ReadOnly]
    public ContactFilter2D contactFilter;

    [ReadOnly]
    //public GameObject AttackCollider;
    public Collider2D[] hitColliders;

    [ReadOnly]
    public Rigidbody2D rb2d;

    [ReadOnly]
    public SpriteRenderer spriteRenderer;
    [ReadOnly]
    public SpriteRenderer offenseStateSpriteRenderer;

    [ReadOnly]
    public LayerMask deadLayer;
    [ReadOnly]
    public LayerMask liveLayer;
    
    [ReadOnly]
    public Color enemyIsOnAttack;

    public AudioClip[] atkClip;
    public AudioClip[] healClip;
    public AudioClip[] idleClip;
    
    [ReadOnly]
    public GameObject attackAnimation;

    [HideInInspector]public GameObject pointGoto;
    
    private Color enemyStandardColor;
    private Color enemyAttackColor;
    private Color enemyStunnedColor;
    //private Color enemyDamagedColor;
    private Color enemyOffenseStateStandardColor;

    private bool callAlreadyCheckTheFlip = false;

    private Coroutine coroutine = null;

    private Blink blink;

    private List<GameObject> list;
    
    [HideInInspector]public bool instantiateOnlyOne = false;

    [Title("Modifiche abilitate.", "$MyTitle")]
    public string MyTitle = "piccolo test, puoi scriverci quello che vuoi :D";

    public enum EnemyState
    {
        idle,
        attack,
        heal,
        searchPlayer,
        escape,
        stun
    }

    private void OnDestroy()
    {
        if(gameManager != null)
        {
            gameManager.GetComponent<EnemyManager>().Remove(this.gameObject);
            
            if(list != null)
            {
                list.Remove(this.gameObject);
            }
        }
    }

    void Awake()
    {
        
    }

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        //offenseState = offenseState.GetComponent<GameObject>();
        gameManager = GameObject.Find("GameManager");
        gameManager.GetComponent<EnemyManager>().Add(this.gameObject);
        enemyStandardColor = Color.white;
        enemyAttackColor = new Color(1f, 0.3f, 0f);
        enemyStunnedColor = Color.black;
        //enemyDamagedColor = Color.red;
        enemyIsOnAttack = Color.magenta;
        enemyOffenseStateStandardColor = Color.white;

        spriteRenderer = GetComponent<SpriteRenderer>();
        offenseStateSpriteRenderer = offenseState.GetComponent<SpriteRenderer>();
        blink = GetComponent<Blink>();

        spriteRenderer.color = enemyStandardColor;
        offenseStateSpriteRenderer.color = enemyOffenseStateStandardColor;

        rb2d = gameObject.GetComponent<Rigidbody2D>();

        if(pointGoto == null)
        {
            pointGoto = gameObject;
        }
        //stun.SetActive(false);
        //search the stun gameobject
        /*
        GameObject[] childList;
        childList = this.transform.GetComponentsInChildren<GameObject>();
        foreach(GameObject child in childList)
        {
            if(child.name == "Stun")
            {
                stun = child;
            }
        }*/

        deadLayer = (LayerMask.NameToLayer("corpse"));
        enemyHealth = GetComponent<EnemyHealth>();
        enemyState = EnemyState.idle;
        //Debug.Log(deadLayer.value);
        waited = false;
    }

    void Update()
    {
        switch (enemyState)
        {
            case EnemyState.idle:
                animationEnded = false;
                if (animator != null)
                {
                    animator.SetInteger("State", 0);
                }
                Idle();
                break;

            case EnemyState.attack:
                if (animator != null)
                {
                    animator.SetInteger("State", 1);
                }
                Attack();
                break;

            case EnemyState.heal:
                if (animator != null)
                {
                    animator.SetInteger("State", 2);
                }
                Healing();
                break;

            case EnemyState.searchPlayer:
                /*if (animator != null)
                {
                    animator.SetInteger("State", 3);
                }*/
                SearchPlayer();
                break;

            case EnemyState.escape:
                /*if (animator != null)
                {
                    animator.SetInteger("State", 4);
                }*/
                Escape();
                break;

            case EnemyState.stun:
                if (animator != null)
                {
                    animator.SetInteger("State", 3);
                }
                Stun();
                break;

            default:
                break;
        }

        //Debug.Log("waited: " + waited);
        if (enemyHealth.health <= 0 && onlyOneDeath)
        {
            onlyOneDeath = false;
            CheckIfCanStunAnotherTime();
            enemyState = EnemyState.stun;
        }

        if (activeStunTime)
        {
            if (stunnedTime < stunTime && enemyState == EnemyState.stun)
            {
                stunnedTime += Time.deltaTime;
                //Debug.Log(stunnedTime);
            }
            else if (stunnedTime >= stunTime && enemyState == EnemyState.stun)
            {
                //Debug.Log("fine stun______________________" + stunnedTime);
                regenerate = true;
                spriteRenderer.color = enemyStandardColor;
                enemyState = EnemyState.idle;
                gameObject.tag = "Enemy";
                gameObject.layer = deadLayer;
                stunnedTime = 0;
            }
        }

        if (enemyState != EnemyState.stun && instantiateOnlyOne)
        {
            DeactivateStun();
        }

        if(enemyState == EnemyState.idle || enemyState == EnemyState.searchPlayer || enemyState == EnemyState.escape)
        {
            //Debug.Log("...........................");
            animator.SetFloat("Velocity", rb2d.velocity.magnitude);
        }
        else
        {
            animator.SetFloat("Velocity", 0);
        }

        /*
        if (stardCountdown)
        {
            if (delay >= 0)
            {
                delay -= Time.deltaTime;
            }
            if (delay < 0)
            {
                delay = 0;
                stardCountdown = false;
            }
        }*/
        if(idleClip.Length > 0 && Time.timeScale != 0)
        {
            if (enemyState == EnemyState.idle || enemyState == EnemyState.searchPlayer || enemyState == EnemyState.escape || enemyState == EnemyState.stun)
            {
                if (gameObject.GetComponent<AudioSource>().name != idleClip[0].name)
                {
                    if (!gameObject.GetComponent<AudioSource>().isPlaying)
                    {
                        gameObject.GetComponent<AudioSource>().clip = idleClip[0];
                        gameObject.GetComponent<AudioSource>().Play();
                    }
                }
            }
        }

        if(!arrivedToThePoint && (enemyState == EnemyState.idle || enemyState == EnemyState.searchPlayer))
        {
            MoveToThePoint();
        }
        //print(enemyState);
    }

    public virtual void Idle()
    {/*
        if(spriteRenderer.color != enemyDamagedColor)
        {
            spriteRenderer.color = enemyStandardColor;
        }*/
    }

    public virtual void Attack()
    {
        if(!alreadyInAttack)
        {
            waited = false;
            StartCoroutine(Wait(attackDelay));
        }
        blink.DoBlink(enemyAttackColor, enemyOffenseStateStandardColor, 10, offenseState);
        //offenseStateSpriteRenderer.color = enemyAttackColor;
    }

    public virtual void Healing()
    {

    }

    public virtual void SearchPlayer()
    {
        //offenseStateSpriteRenderer.color = enemyOffenseStateStandardColor;
        if (doPlayerDamage)
        {
            /*
            if (!alreadyInAttack)
            {
                StartCoroutine(Wait(attackDelay));
            }*/
            enemyState = EnemyState.attack;
        }
    }

    public virtual void Escape()
    {
        offenseStateSpriteRenderer.color = enemyOffenseStateStandardColor;
        delay = attackDelay;
        stardCountdown = false;
    }

    public virtual void Stun()
    {
        //animazione stun
        ActivateStun(stun, 3, 3);

        rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        //Debug.Log("name:" + spriteRenderer.name);
        spriteRenderer.color = enemyStunnedColor;
        gameObject.tag = "Corpse";
        gameObject.layer = deadLayer;
    }

    //not state machine things

    public virtual void AddToList(List<GameObject> thisList)
    {
        list = thisList;
    }

    public virtual void Flip()
    {
        //Debug.Log("Terno");
        callAlreadyCheckTheFlip = false;
        facingRight = !facingRight;
        Vector3 normalScale = transform.localScale;
        normalScale.x *= -1;
        transform.localScale = normalScale;
    }

    public virtual void CheckForFlip()
    {
        //il nemico si gira verso il player
        if (enemyState != EnemyState.stun)
        {
            if (!callAlreadyCheckTheFlip)
            {
                if (player.transform.position.x < this.transform.position.x && facingRight)
                {
                    callAlreadyCheckTheFlip = true;
                    coroutine = StartCoroutine(WaitBeforeFlip(waitTimeBeforeFlip, timeBeforeStoppedCoroutine));
                }
                if (player.transform.position.x > this.transform.position.x && !facingRight)
                {
                    callAlreadyCheckTheFlip = true;
                    coroutine = StartCoroutine(WaitBeforeFlip(waitTimeBeforeFlip, timeBeforeStoppedCoroutine));
                }
            }

            if (coroutine != null)
            {
                if (player.transform.position.x < this.transform.position.x && !facingRight)
                {
                    StopCoroutine(coroutine);
                    callAlreadyCheckTheFlip = false;
                    coroutine = null;
                }
                if (player.transform.position.x > this.transform.position.x && facingRight)
                {
                    StopCoroutine(coroutine);
                    callAlreadyCheckTheFlip = false;
                    coroutine = null;
                }
            }
        }
    }

    private void IsPlayerdamageable(bool isDamageable)
    {
        //Debug.Log("canDamagePlayer");
        doPlayerDamage = isDamageable;
    }

    public void ActivateStun(GameObject obj, float x, float y)
    {
        if(!instantiateOnlyOne)
        {
            instantiateOnlyOne = true;
            stun.SetActive(true);
        }
    }

    public void DeactivateStun()
    {
        instantiateOnlyOne = false;
        stun.SetActive(false);
    }

    public void CheckIfCanStunAnotherTime()
    {
        if(TimeCanDeath <= 0)
        {
            //DeathAnimation------------------------------------------------------------------------------------------------------------------
            Destroy(gameObject);
        }
        else
        {
            TimeCanDeath -= 1;
        }
    }

    public void MoveToThePoint()
    {
        if(IsNear(gameObject.transform.position.x, pointGoto.transform.position.x))
        {
            direction = gameObject.transform.position.x - pointGoto.transform.position.x;

            rb2d.velocity = new Vector2(-Mathf.Sign(direction) * speed * 200, rb2d.velocity.y);
        }
    }

    public bool IsNear(float point1, float point2)
    {
        if(Mathf.Abs(point1 - point2) > 50)
        {
            return true;
        }
        else
        {
            arrivedToThePoint = true;
            return false;
        }
    }

    //****************************************************//
    //things 4 animator

    public virtual void AttackForAnimator()
    {
        if (doPlayerDamage)
        {
            player.SendMessage("Damage", damage);
        }
    }

    public virtual void IsAnimationEnded()
    {
        animationEnded = true;
    }

    //****************************************************//

    public virtual IEnumerator WaitBeforeFlip(float time, float timeBeforeStop)
    {
        if(dontResetFlipCountdown)
        {
            timeBeforeStoppedCoroutine += Time.deltaTime;
        }
        yield return new WaitForSeconds(time - timeBeforeStop);
        timeBeforeStoppedCoroutine = 0f;
        Flip();
    }

    public IEnumerator Wait(float sec)
    {
        Debug.Log("waiting");
        alreadyInAttack = true;
        if (atkClip.Length > 1)
        {
            gameObject.GetComponent<AudioSource>().clip = atkClip[1];
            gameObject.GetComponent<AudioSource>().Play();
        }
        
        if(animator == null)
        {
            yield return new WaitForSeconds(sec);
        }
        else
        {
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }
        if (atkClip.Length > 0)
        {
            gameObject.GetComponent<AudioSource>().clip = atkClip[0];
            gameObject.GetComponent<AudioSource>().Play();
        }
        alreadyInAttack = false;
        waited = true;
    }
}