﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[RequireComponent(typeof(EnemyStateMachine))]
[RequireComponent(typeof(EnemyHealth))]
public class EnemyTank : EnemyStateMachine
{
    [HideInInspector] public bool facingleft = true;
    //[HideInInspector] public bool rotate = false;

    public int tankDamage = 10;
    public int tankHealth = 100;
    public float flipDelay = .1f;

    public float offenceArea = 10f;
    public float maxVisibleDistance = 5f;
    public float tankAttackDelay = 2f;

    public Collider2D shieldCollider;
    public GameObject atkCollider;

    private int i = 0;
    private bool playerIsInVision;

    private bool healtToSet = true;

    

    private void Awake()
    {
        damage = tankDamage;
        attackDelay = tankAttackDelay;
        delay = tankAttackDelay;
        waitTimeBeforeFlip = flipDelay;

        hitColliders = atkCollider.GetComponents<Collider2D>();
        //hitColliders = new Collider2D[maxArray];
    }

    private void FixedUpdate()
    {
        //il nemico si gira verso il player
        CheckForFlip();

        //Debug.Log("Me update");
        //Debug.Log(delay);
        if (healtToSet)
        {
            enemyHealth.SetHealth(tankHealth);
            healtToSet = false;
        }

        if (regenerate)
        {
            //Debug.Log("Regenerate");
            enemyHealth.Heal(healthRegenAfterStun);
            regenerate = false;
            onlyOneDeath = true;
        }

        if (facingleft == false)
        {
            Flip();
        }

    }

    public override void Idle()
    {
        base.Idle();

        enemyState = EnemyState.searchPlayer;

    }

    public override void Attack()
    {
        /*
        if (waited)
        {
            Debug.Log("Attack");
            GameObject atk = Instantiate(attackAnimation, this.transform);
            atk.transform.localScale = new Vector2((atk.transform.localScale.x * 10) / 2, (atk.transform.localScale.y * 10) / 2);
            if (doPlayerDamage)
            {
                player.SendMessage("Damage", damage);
                shieldCollider.enabled = false;
                player.GetComponent<Rigidbody2D>().AddForce(new Vector2(-(Mathf.Sign(gameObject.transform.position.x - player.transform.position.x)) * 1000, 0), ForceMode2D.Impulse);
                //StartCoroutine(WaitToReactivateShieldCollider());
            }
            waited = false;
        }
        
        if (!alreadyInAttack)
        {
            Debug.Log("Attack2");
            enemyState = EnemyState.searchPlayer;
        }*/

        base.Attack();
        if (player.GetComponentInChildren<Life>().actualLife <= 0)
        {
            GameObject.Find("DeathBy").GetComponent<LastEnemyDamage>().KilledBy("Tank");
        }
    }

    public override void SearchPlayer()
    {
        base.SearchPlayer();

        if (playerIsInVision)
        {
            direction = gameObject.transform.position.x - player.transform.position.x;

            rb2d.velocity = new Vector2(-Mathf.Sign(direction) * speed * 200, rb2d.velocity.y);
        }
        /*
        if (doPlayerDamage)
        {
            if(!alreadyInAttack)
            {
                StartCoroutine(Wait(attackDelay));
            }
            enemyState = EnemyState.attack;
        }*/
    }

    private void IsPlayerIn(bool isIn)
    {
        playerIsInVision = isIn;
    }
    /*
	public void Flip()
	{
		//Debug.Log("Do Flip");
		facingleft = !facingleft;
		Vector3 normalScale = transform.localScale;
		normalScale.x *= -1;
		transform.localScale = normalScale;
	}*/

    IEnumerator WaitToReactivateShieldCollider()
    {
        yield return new WaitForSeconds(.5f);
        shieldCollider.enabled = true;
    }
}