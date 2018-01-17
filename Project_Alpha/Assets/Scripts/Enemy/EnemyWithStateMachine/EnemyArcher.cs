using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStateMachine))]
[RequireComponent(typeof(EnemyHealth))]
public class EnemyArcher : EnemyStateMachine
{
    private int archerDamage = 15;
    private float offsetEscape = 50f;
    public int archerHealth = 50;

    public float escapeAreaDistance = 2000f;
    public float maxVisibleDistance = 5000f;
    public float archerAttackDelay = 1f;
    //public float arrowVelocity = 50f;
    public GameObject arrowPrefab;
    public GameObject player;
    public GameObject attackCollider;
    public GameObject escapeArea;

    //private float delay = 0;
    private int i = 0;
    private bool healtToSet = true;
    private bool playerIsDamageable = false;//if the player is in the first area(attack)
    private bool thereIsAPlayer = false;//if the player is in the second area(run)
    private RaycastHit2D playerInLine;//check if the player is in line with the archer

    private void Awake()
    {
        damage = archerDamage;
        attackDelay = archerAttackDelay;
        delay = archerAttackDelay;
        //areaAttack.SendMessage("SetWaitTime", archerAttackDelay);

        attackCollider.GetComponent<CircleCollider2D>().radius = maxVisibleDistance;
        escapeArea.GetComponent<CircleCollider2D>().radius = escapeAreaDistance;

        //hitColliders = new Collider2D[maxArray];
    }

    private void FixedUpdate()
    {
        if(healtToSet)
        {
            enemyHealth.SetHealth(archerHealth);
            healtToSet = false;
        }
        
        if(regenerate)
        {
            enemyHealth.Heal(healthRegenAfterStun);
            regenerate = false;
            onlyOneDeath = true;
        }

        if(player == null)
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

        //create and shoot arrow
        if(waited)
        {
            waited = false;
            StartCoroutine(Wait(attackDelay));
            //creare successivamente la parabola della freccia
            Instantiate(arrowPrefab, gameObject.transform.position, Quaternion.identity);
        }

        enemyState = EnemyState.idle;
    }

    public override void SearchPlayer()
    {
        base.SearchPlayer();
        
        if(playerIsDamageable)
        {
            enemyState = EnemyState.attack;
        }

        if(thereIsAPlayer)
        {
            enemyState = EnemyState.escape;
        }
    }

    public override void Escape()
    {
        base.Escape();

        Debug.Log("Ruun forrest, ruuuuuuuuuuuuuuunnnnnnnnnnnn!!!!!!");

        if(thereIsAPlayer)
        {
            direction = gameObject.transform.position.x - player.transform.position.x + offsetEscape;

            rb2d.velocity = new Vector2(Mathf.Sign(direction) * speed * 200, rb2d.velocity.y);
        }

        enemyState = EnemyState.searchPlayer;
        
    }

    private void IsPlayerIn(bool isIn)
    {
        playerIsDamageable = isIn;
    }

    private void IsPlayerTooNear(bool isIn)
    {
        thereIsAPlayer = isIn;
    }

    IEnumerator Wait(float sec)
    {
        //Debug.Log("waiting");
        yield return new WaitForSeconds(sec);
        waited = true;
    }
}

