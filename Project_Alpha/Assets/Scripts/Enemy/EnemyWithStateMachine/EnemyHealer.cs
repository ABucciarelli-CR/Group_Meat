using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStateMachine))]
[RequireComponent(typeof(EnemyHealth))]
public class EnemyHealer : EnemyStateMachine
{
    private bool playerIsInVision = false;
    private bool doPlayerDamage = false;
    private bool enemyIsIn = false;
    private bool healtToSet = true;

    public int healerDamage = 5;
    public int healerHeal = 10;
    public int healerHealth = 50;
    public float healerSpeed = .5f;
    public float healerAttackDelay = 5f;
    public float healerHealDelay = 3f;
    public float maxAttackDistance = 3000f;
    public float maxVisibleDistance = 4000f;

    public GameObject attackCollider;
    public GameObject visibleDistanceCollider;
    public GameObject healDistanceCollider;
    public GameObject player;


    private void Awake()
    {
        damage = healerDamage;
        attackDelay = healerAttackDelay;
        speed = healerSpeed;
        //areaAttack.SendMessage("SetWaitTime", archerAttackDelay);

        attackCollider.GetComponent<CircleCollider2D>().radius = maxAttackDistance;
        visibleDistanceCollider.GetComponent<CircleCollider2D>().radius = maxVisibleDistance;
        healDistanceCollider.GetComponent<CircleCollider2D>().radius = maxVisibleDistance;

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

    }

    public override void Idle()
    {
        base.Idle();

        //Debug.Log("To search");
        enemyState = EnemyState.searchPlayer;

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

    public override void SearchPlayer()
    {
        base.SearchPlayer();

        if (doPlayerDamage)
        {
            enemyState = EnemyState.attack;
        }

        if (playerIsInVision && !enemyIsIn)
        {
            direction = gameObject.transform.position.x - player.transform.position.x;

            rb2d.velocity = new Vector2(-Mathf.Sign(direction) * speed * 200, rb2d.velocity.y);
        }
    }

    private void IsPlayerIn(bool isIn)
    {
        playerIsInVision = isIn;
    }

    private void IsEnemyIn(bool isEnemyIn)
    {
        enemyIsIn = isEnemyIn;
    }

    private void IsPlayerdamageable(bool isDamageable)
    {
        Debug.Log("canDamagePlayer");
        doPlayerDamage = isDamageable;
    }

    IEnumerator Wait(float sec)
    {
        //Debug.Log("waiting");
        yield return new WaitForSeconds(sec);
        waited = true;
    }
}
