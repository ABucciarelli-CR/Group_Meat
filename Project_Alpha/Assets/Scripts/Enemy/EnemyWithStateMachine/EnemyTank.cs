using System.Collections;
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

    public GameObject atkCollider;
    
    private int i = 0;

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
			Flip ();
		}

    }

    public override void Idle()
    {
        base.Idle();

        ttcd += Time.deltaTime;
        //Debug.Log("Me idle");
        if (ttcd >= timeToChangeDirection)
        {
            ttcd = 0;
            direction *= -1;
        }
        movement = new Vector2(direction * speed * 2, 0);
        //gameObject.transform.Translate(movement);
        rb2d.MovePosition(rb2d.position + movement);

        enemyState = EnemyState.searchPlayer;

    }

    public override void Attack()
    {
        base.Attack();

        if (waited)
        {
            GameObject atk = Instantiate(attackAnimation, this.transform);
            atk.transform.localScale = new Vector2((atk.transform.localScale.x * 10) / 2, (atk.transform.localScale.y * 10) / 2);
            if(doPlayerDamage)
            {
                player.SendMessage("Damage", damage);
            }
            waited = false;
        }
        
        if(!alreadyInAttack)
        {
            enemyState = EnemyState.searchPlayer;
        }
    }

    public override void SearchPlayer()
    {
        base.SearchPlayer();
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
    /*
	public void Flip()
	{
		//Debug.Log("Do Flip");
		facingleft = !facingleft;
		Vector3 normalScale = transform.localScale;
		normalScale.x *= -1;
		transform.localScale = normalScale;
	}*/
}

