using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(EnemyStateMachine))]
[RequireComponent(typeof(EnemyHealth))]
public class EnemyTank : EnemyStateMachine
{
    public int tankDamage = 100;
    public int tankHealth = 80;

    public float offenceArea = 10f;
    public float maxVisibleDistance = 5f;
    public float tankAttackDelay = 2f;
    

    private float delay = 0;
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

        if (delay >= 0)
        {
            delay -= Time.fixedDeltaTime;
        }
        if (delay < 0)
        {
            delay = 0;
        }
    }

    public override void Idle()
    {
        ttcd += Time.deltaTime;
        //Debug.Log("Me idle");
        if (ttcd >= timeToChangeDirection)
        {
            ttcd = 0;
            direction *= -1;
        }
        movement = new Vector2(direction * speed, 0);
        gameObject.transform.Translate(movement);

        enemyState = EnemyState.searchPlayer;

    }

    public override void Attack()
    {
        //Debug.Log("damaging");
        hitColliders[i].gameObject.SendMessage("Damage", damage);
        i = 0;
        delay = attackDelay;
        System.Array.Clear(hitColliders, 0, maxArray);
        enemyState = EnemyState.searchPlayer;
    }

    public override void SearchPlayer()
    {
        Physics2D.OverlapCircle(transform.position, maxVisibleDistance, contactFilter, hitColliders);
        //Debug.DrawRay(transform.position, Vector2.left, Color.green, maxVisibleDistance);
        //Debug.Log("Me collide");
        foreach (Collider2D collider in hitColliders)
        {

            if (hitColliders[i] != null)
            {
                //Debug.Log("Hitted" + i);
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
}

