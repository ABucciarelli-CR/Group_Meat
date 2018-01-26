using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Blink))]
public class EnemyStateMachine : MonoBehaviour
{

    [HideInInspector] public EnemyState enemyState;
    
    public float speed = .8f;
    public int damage;
    public float attackDelay;
    public float healDelay;
    [HideInInspector] public Vector2 movement;
    [HideInInspector] public float direction = 1;
    [HideInInspector] public float timeToChangeDirection = 5f;
    [HideInInspector] public float ttcd;
    /*[HideInInspector]*/
    
    public float stunTime = 5f;
    public int healthRegenAfterStun = 20;//is in %
    public bool activeStunTime = true;
    [HideInInspector] public float delay;
    [HideInInspector] public int maxArray = 100;
    [HideInInspector] public bool stardCountdown = true;
    [HideInInspector] public bool regenerate = false;//to abilitate the enemy regeneration afrer stun
    [HideInInspector] public bool onlyOneDeath = true; //ceck to not die several times
    [HideInInspector] public bool waited = true;

    [HideInInspector] public EnemyHealth enemyHealth;
    [HideInInspector] public GameObject gameManager;
    [HideInInspector] public float stunnedTime = 0;

    public GameObject offenseState;//the gameObject that visualize
    

    public ContactFilter2D contactFilter;

    //public GameObject AttackCollider;
    public Collider2D[] hitColliders;

    public Rigidbody2D rb2d;

    public SpriteRenderer spriteRenderer;
    public SpriteRenderer offenseStateSpriteRenderer;

    public LayerMask deadLayer;
    public LayerMask liveLayer;

    public Color enemyIsOnAttack;

    private Color enemyStandardColor;
    private Color enemyAttackColor;
    private Color enemyStunnedColor;
    //private Color enemyDamagedColor;
    private Color enemyOffenseStateStandardColor;

    private Blink blink;

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
        }
    }

    void Awake()
    {
        
    }

    void Start()
    {
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

        deadLayer = (LayerMask.NameToLayer("corpse"));
        enemyHealth = GetComponent<EnemyHealth>();
        enemyState = EnemyState.idle;
        //Debug.Log(deadLayer.value);
    }

    void Update()
    {
        /*
        if(spriteRenderer == null && offenseStateSpriteRenderer == null)
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            offenseStateSpriteRenderer = offenseState.GetComponent<SpriteRenderer>();

            
        }*/


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
        }


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

    public virtual IEnumerator WaitTime(float time)
    {
        Debug.Log("waiting");
        yield return new WaitForSeconds(time);
        Debug.Log("stop waiting");
    }

}


