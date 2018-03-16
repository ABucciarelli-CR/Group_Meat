using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Blink))]
public class EnemyStateMachine : MonoBehaviour
{
    [HideInInspector] public EnemyState enemyState;

    [Title("ReadOnly, modifiche disabilitate.")]
    [ReadOnly]
    public float speed = .8f;
    [ReadOnly]
    public int damage;
    [ReadOnly]
    public float attackDelay;
    [ReadOnly]
    public float healDelay;
    [HideInInspector] public Vector2 movement;
    [HideInInspector] public float direction = 1;
    [HideInInspector] public float timeToChangeDirection = 5f;
    [HideInInspector] public float ttcd;
    [HideInInspector] public bool doPlayerDamage = false;
    //[ReadOnly]
    public bool facingRight = false;
    /*[HideInInspector]*/

    [ReadOnly]
    public float stunTime = 5f;
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

    //[ReadOnly]
    public GameObject attackAnimation;
    
    private Color enemyStandardColor;
    private Color enemyAttackColor;
    private Color enemyStunnedColor;
    //private Color enemyDamagedColor;
    private Color enemyOffenseStateStandardColor;

    private bool callAlreadyCheckTheFlip = false;

    private Coroutine coroutine = null;

    private Blink blink;

    private List<GameObject> list;

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
        //offenseState = offenseState.GetComponent<GameObject>();
        gameManager = GameObject.Find("GameManager");
        player = GameObject.FindWithTag("Player");
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

        deadLayer = (LayerMask.NameToLayer("corpse"));
        enemyHealth = GetComponent<EnemyHealth>();
        enemyState = EnemyState.idle;
        //Debug.Log(deadLayer.value);
    }

    void Update()
    {
        //Debug.Log("waited: " + waited);
        if (enemyHealth.health <= 0 && onlyOneDeath)
        {
            onlyOneDeath = false;
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
                regenerate = true;
                spriteRenderer.color = enemyStandardColor;
                enemyState = EnemyState.idle;
                gameObject.tag = "Enemy";
                gameObject.layer = deadLayer;
                stunnedTime = 0;
            }
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


        switch (enemyState)
        {
            case EnemyState.idle:
                Idle();
                break;

            case EnemyState.attack:
                Attack();
                break;

            case EnemyState.heal:
                Healing();
                break;

            case EnemyState.searchPlayer:
                SearchPlayer();
                break;

            case EnemyState.escape:
                Escape();
                break;

            case EnemyState.stun:
                Stun();
                break;

            default:
                break;
        }
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
            if (!alreadyInAttack)
            {
                StartCoroutine(Wait(attackDelay));
            }
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
        //Debug.Log("name:" + spriteRenderer.name);
        spriteRenderer.color = enemyStunnedColor;
        gameObject.tag = "Corpse";
        gameObject.layer = deadLayer;
    }

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
        if (!callAlreadyCheckTheFlip)
        {
            if (player.transform.position.x < this.transform.position.x && facingRight)
            {
                callAlreadyCheckTheFlip = true;
                coroutine = StartCoroutine(WaitBeforeFlip(waitTimeBeforeFlip));
            }
            if (player.transform.position.x > this.transform.position.x && !facingRight)
            {
                callAlreadyCheckTheFlip = true;
                coroutine = StartCoroutine(WaitBeforeFlip(waitTimeBeforeFlip));
            }
        }

        if(coroutine != null)
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

    private void IsPlayerdamageable(bool isDamageable)
    {
        //Debug.Log("canDamagePlayer");
        doPlayerDamage = isDamageable;
    }

    public virtual IEnumerator WaitBeforeFlip(float time)
    {
        yield return new WaitForSeconds(time);
        Flip();
    }

    public virtual IEnumerator WaitTime(float time)
    {
        Debug.Log("waiting");
        yield return new WaitForSeconds(time);
        Debug.Log("stop waiting");
    }

    public IEnumerator Wait(float sec)
    {
        //Debug.Log("waiting");
        alreadyInAttack = true;
        yield return new WaitForSeconds(sec);
        alreadyInAttack = false;
        waited = true;
    }
}


