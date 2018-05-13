using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(EnemyStateMachine))]
[RequireComponent(typeof(EnemyHealth))]
public class EnemyHound : EnemyStateMachine
{
    private bool playerIsInVision = false;
    private bool healtToSet = true;
    public float flipDelay = .1f;

    public int houndDamage = 10;
    public int houndHealth = 30;
    public float houndSpeed = 1.2f;
    public float houndAttackDelay = 5f;
    public float maxAttackDistance = 3000f;
    public float maxVisibleDistance = 4000f;
    //public float stunned = 5f;

    public GameObject attackCollider;
    public GameObject visibleDistanceCollider;
    //public GameObject player;


    private void Awake()
    {
        damage = houndDamage;
        attackDelay = houndAttackDelay;
        speed = houndSpeed;
        waitTimeBeforeFlip = flipDelay;
        //stunTime = stunned;
        //areaAttack.SendMessage("SetWaitTime", archerAttackDelay);

        attackCollider.GetComponent<CircleCollider2D>().radius = maxAttackDistance;
        visibleDistanceCollider.GetComponent<CircleCollider2D>().radius = maxVisibleDistance;
        animator = gameObject.GetComponent<Animator>();
        //animator.GetComponent<EndAttack>().damage = damage;
        //hitColliders = new Collider2D[maxArray];
    }
    
    private void FixedUpdate()
    {
        /*if(gameManager.GetComponent<GlobalVariables>().SwordsmanAttack == 0)
        {
            gameManager.GetComponent<GlobalVariables>().SwordsmanAttack = damage;
        }*/
        //il nemico si gira verso il player
        if (enemyState == EnemyState.idle || enemyState == EnemyState.searchPlayer)
        {
            CheckForFlip();
        }
        

        if (healtToSet)
        {
            enemyHealth.SetHealth(houndHealth);
            healtToSet = false;
        }

        if (regenerate)
        {
            enemyHealth.Heal(healthRegenAfterStun);
            regenerate = false;
            onlyOneDeath = true;
        }
        /*
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }*/
    }

    public override void Idle()
    {
        base.Idle();

        //Debug.Log("To search");
        enemyState = EnemyState.searchPlayer;

    }

    public override void Attack()
    {
        /*if (waited)
        {
            GameObject atk = Instantiate(attackAnimation, this.transform);
            atk.transform.localScale = new Vector2((atk.transform.localScale.x * 10) / 2, (atk.transform.localScale.y * 10) / 2);
            if (doPlayerDamage)
            {
                player.SendMessage("Damage", damage);
            }
            waited = false;
        }*/

        //base.Attack();
        if (!doPlayerDamage && animationEnded)
        {
            Debug.Log("_______________________");
            enemyState = EnemyState.idle;
        }
    }

    public override void SearchPlayer()
    {
        base.SearchPlayer();
        /*
        if (doPlayerDamage)
        {
            if (!alreadyInAttack)
            {
                StartCoroutine(Wait(attackDelay));
            }
            enemyState = EnemyState.attack;
        }*/

        if (playerIsInVision)
        {
            Debug.Log("Leeeeeeeeeeeeroooooooy JENKINS!!!!");
            direction = gameObject.transform.position.x - player.transform.position.x;

            rb2d.velocity = new Vector2(-Mathf.Sign(direction) * speed * 200, rb2d.velocity.y);
        }
    }

    private void IsPlayerIn(bool isIn)
    {
        playerIsInVision = isIn;
    }
}
