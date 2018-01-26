using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(EnemyStateMachine))]
[RequireComponent(typeof(EnemyHealth))]
public class EnemyTank : EnemyStateMachine
{
    [HideInInspector] public bool facingleft = true;
    //[HideInInspector] public bool rotate = false;

    public int tankDamage = 10;
    public int tankHealth = 80;

    public float offenceArea = 10f;
    public float maxVisibleDistance = 5f;
    public float tankAttackDelay = 2f;
    
    private int i = 0;

    private bool healtToSet = true;

    private void Awake()
    {
        damage = tankDamage;
        attackDelay = tankAttackDelay;
        delay = tankAttackDelay;

        hitColliders = new Collider2D[maxArray];
    }

    private void FixedUpdate()
    {
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

        //Debug.Log("damaging");
        hitColliders[i].gameObject.SendMessage("Damage", damage);
        i = 0;
        delay = attackDelay;
        //WaitTime(tankAttackDelay);

        System.Array.Clear(hitColliders, 0, maxArray);
        enemyState = EnemyState.searchPlayer;
    }

    public override void SearchPlayer()
    {
        base.SearchPlayer();

        Physics2D.OverlapCircle(transform.position, maxVisibleDistance, contactFilter, hitColliders);
        //Debug.DrawRay(transform.position, Vector2.left, Color.green, maxVisibleDistance);
        //Debug.Log("Me collide");
        foreach (Collider2D collider in hitColliders)
        {
            if (hitColliders[i] != null)
            {
                //Debug.Log("Hitted" + i);
                if (hitColliders[i].CompareTag("Player") && !stardCountdown)
                {
                    stardCountdown = true;
                }
                else if(hitColliders[i].CompareTag("Player") && delay <= (attackDelay / 2))
                {
                    offenseStateSpriteRenderer.color = enemyIsOnAttack;
                }

                if (hitColliders[i].CompareTag("Player") && delay == 0)
                {
                    enemyState = EnemyState.attack;
                    break;
                }
                else
                {
                    i++;
                }
            }
            else
            {
                break;
            }
        }
        //Debug.Log("Exit");
        if (enemyState == EnemyState.searchPlayer)
        {
            enemyState = EnemyState.idle;
            i = 0;
            System.Array.Clear(hitColliders, 0, maxArray);
        }
    }



	public void Flip()
	{
		//Debug.Log("Do Flip");
		facingleft = !facingleft;
		Vector3 normalScale = transform.localScale;
		normalScale.x *= -1;
		transform.localScale = normalScale;
	}
}

