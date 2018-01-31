﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStateMachine))]
[RequireComponent(typeof(EnemyHealth))]
public class EnemyHealer : EnemyStateMachine
{
    private int maxEnemyInList = 20;
    private bool doPlayerDamage = false;
    private bool healtToSet = true;
    private bool onlyHealer = false;

    public int healerDamage = 5;
    public int healerHeal = 10;
    public int healerHealth = 50;
    public float healerSpeed = .5f;
    public float healerAttackDelay = 5f;
    public float healerHealDelay = 3f;
    public float maxAttackDistance = 3000f;

    public GameObject attackCollider;
    public GameObject player;


    private void Awake()
    {
        damage = healerDamage;
        attackDelay = healerAttackDelay;
        healDelay = healerHealDelay;
        speed = healerSpeed;
        //areaAttack.SendMessage("SetWaitTime", archerAttackDelay);

        attackCollider.GetComponent<CircleCollider2D>().radius = maxAttackDistance;

        //hitColliders = new Collider2D[maxArray];
    }

    private void FixedUpdate()
    {
        if (healtToSet)
        {
            enemyHealth.SetHealth(healerHealth);
            healtToSet = false;
        }

        if (regenerate)
        {
            enemyHealth.Heal(healthRegenAfterStun);
            regenerate = false;
            onlyOneDeath = true;
        }

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        onlyHealer = true;

        foreach (GameObject thisEnemy in gameManager.GetComponent<EnemyManager>().enemy)
        {
            if(thisEnemy.name != "Healer" || thisEnemy.name != "Healer(Clone)")
            {
                onlyHealer = false;
                break;
            }
        }
    }

    public override void Idle()
    {
        base.Idle();

        //Debug.Log("To search");

        if (onlyHealer && doPlayerDamage)
        {
            enemyState = EnemyState.attack;
        }
        else if(onlyHealer && !doPlayerDamage)
        {
            enemyState = EnemyState.heal;
        }
        else if(!onlyHealer)
        {
            enemyState = EnemyState.heal;
        }
        else
        {
            enemyState = EnemyState.searchPlayer;
        }
    }

    public override void Attack()
    {
        base.Attack();

        if (waited)
        {
            player.SendMessage("Damage", damage);
            waited = false;
            StartCoroutine(Wait(attackDelay));
        }

        enemyState = EnemyState.idle;
    }

    public override void Healing()
    {
        base.Healing();
        
        if (waited)
        {
            foreach (GameObject thisEnemy in gameManager.GetComponent<EnemyManager>().enemy)
            {
                //Debug.Log("Healing: " + thisEnemy.name);
                thisEnemy.SendMessage("HealIfAlive", healerHeal);
            }
            waited = false;
            StartCoroutine(Wait(healDelay));
        }

        enemyState = EnemyState.idle;
    }

    public override void SearchPlayer()
    {
        base.SearchPlayer();

        if (doPlayerDamage)
        {
            enemyState = EnemyState.attack;
        }

        if (onlyHealer)
        {
            direction = gameObject.transform.position.x - player.transform.position.x;

            rb2d.velocity = new Vector2(-Mathf.Sign(direction) * speed * 200, rb2d.velocity.y);
        }
    }
    
    private void IsPlayerdamageable(bool isDamageable)
    {
        //Debug.Log("canDamagePlayer");
        doPlayerDamage = isDamageable;
    }

    IEnumerator Wait(float sec)
    {
        //Debug.Log("waiting");
        yield return new WaitForSeconds(sec);
        waited = true;
    }
}