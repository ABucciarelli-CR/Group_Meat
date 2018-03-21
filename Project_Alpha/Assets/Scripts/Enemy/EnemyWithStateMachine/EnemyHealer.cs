using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(EnemyStateMachine))]
[RequireComponent(typeof(EnemyHealth))]
public class EnemyHealer : EnemyStateMachine
{
    private int maxEnemyInList = 20;
    private bool healtToSet = true;
    private bool onlyHealer = false;
    private bool canCheckHealer = false;
    public float flipDelay = .1f;

    public int healerDamage = 5;
    public int healerHeal = 10;
    public int healerHealth = 60;
    public float healerSpeed = .5f;
    public float healerAttackDelay = 5f;
    public float healerHealDelay = .5f;
    public float maxAttackDistance = 3000f;

    [Title("ReadOnly, modifiche disabilitate.")]
    [ReadOnly]
    public GameObject attackCollider;
    //[ReadOnly]
    //public GameObject player;
    [ReadOnly]
    public GameObject disappearingPlatform;
    [ReadOnly]
    public GameObject healingAnimation;

    private bool callFinalOnlyone = true;

    private void Awake()
    {
        damage = healerDamage;
        attackDelay = healerAttackDelay;
        healDelay = healerHealDelay;
        speed = healerSpeed;
        waitTimeBeforeFlip = flipDelay;
        //areaAttack.SendMessage("SetWaitTime", archerAttackDelay);

        attackCollider.GetComponent<CircleCollider2D>().radius = maxAttackDistance;
        gameManager = GameObject.Find("GameManager");
        disappearingPlatform = GameObject.Find("DisapperingFloor"); ;
        StartCoroutine(WaitForSpawn(5f));
        //hitColliders = new Collider2D[maxArray];
    }

    private void FixedUpdate()
    {
        //il nemico si gira verso il player
        CheckForFlip();

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
        /*
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }*/

        onlyHealer = true;

        foreach (GameObject thisEnemy in gameManager.GetComponent<EnemyManager>().enemy)
        {
            if(thisEnemy.name != "Healer" && thisEnemy.name != "Healer(Clone)")
            {
                onlyHealer = false;
                break;
            }
        }

        if(onlyHealer && disappearingPlatform != null && canCheckHealer)
        {
            disappearingPlatform.SetActive(false);
            if(callFinalOnlyone)
            {
                callFinalOnlyone = false;
                StartCoroutine(WaitForFinal());
            }
        }
        else if(onlyHealer && disappearingPlatform == null && canCheckHealer)
        {
            if (callFinalOnlyone)
            {
                callFinalOnlyone = false;
                StartCoroutine(WaitForFinal());
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
                Instantiate(healingAnimation, thisEnemy.gameObject.transform.position, Quaternion.identity);
            }
            waited = false;
            StartCoroutine(Wait(healDelay, true));
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

    IEnumerator WaitForSpawn(float sec)
    {
        //Debug.Log("waiting");
        yield return new WaitForSeconds(sec);
        canCheckHealer = true;
    }

    IEnumerator WaitForFinal()
    {
        yield return new WaitForSeconds(1f);
        gameManager.GetComponent<GameManagerAction>().SendMessage("FinalPause");
    }

    IEnumerator Wait(float sec, bool isHeal = false)
    {
        //Debug.Log("waiting");
        yield return new WaitForSeconds(sec);
        if (atkClip.Length > 0 && isHeal)
        {
            gameObject.GetComponent<AudioSource>().clip = atkClip[0];
            gameObject.GetComponent<AudioSource>().Play();
        }
        waited = true;
    }
}
