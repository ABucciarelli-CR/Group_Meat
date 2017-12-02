using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{

    [HideInInspector] public EnemyState enemyState;


    public float speed = .01f;
    public int damage;
    public float attackDelay;
    public ContactFilter2D contactFilter;
    [HideInInspector] public Vector2 movement;
    [HideInInspector] public float direction = 1;
    [HideInInspector] public float timeToChangeDirection = 5f;
    [HideInInspector] public float ttcd;
    /*[HideInInspector]*/
    public Collider2D[] hitColliders;
    [HideInInspector] public int maxArray = 100;
    public LayerMask deadLayer;
    public LayerMask liveLayer;
    public float stunTime = 5f;
    public int healthRegenAfterStun = 20;//is in %
    public bool activeStunTime = true;
    [HideInInspector] public bool regenerate = false;//to abilitate the enemy regeneration afrer stun
    [HideInInspector] public bool onlyOneDeath = true; //ceck to not die several times

    [HideInInspector] public EnemyHealth enemyHealth;
    [HideInInspector] public float stunnedTime = 0;
        


    public enum EnemyState
    {
        idle,
        attack,
        searchPlayer,
        escape,
        stun
    }

    void Awake()
    {
            
    }

    void Start()
    {
        deadLayer = (LayerMask.NameToLayer("corpse"));
        enemyHealth = GetComponent<EnemyHealth>();
        enemyState = EnemyState.idle;
        //Debug.Log(deadLayer.value);
    }

    void Update()
    {
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
                enemyState = EnemyState.idle;
                gameObject.tag = "Enemy";
                gameObject.layer = deadLayer;
                stunnedTime = 0;
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
    { }

    public virtual void Attack()
    { }

    public virtual void SearchPlayer()
    { }

    public virtual void Escape()
    { }

    public virtual void Stun()
    {
        gameObject.tag = "Corpse";
        gameObject.layer = deadLayer;
    }

}


