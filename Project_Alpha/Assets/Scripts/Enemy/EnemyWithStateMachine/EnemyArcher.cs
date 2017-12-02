using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStateMachine))]
public class EnemyArcher : EnemyStateMachine
{
    public float escapeArea = 10f;
    public float maxVisibleDistance = 50f;
    public int archerDamage = 100;
    public float archerAttackDelay = 1f;
    //public float arrowVelocity = 50f;
    public GameObject arrowPrefab;
    public GameObject player;

    private float delay = 0;
    private int i = 0;
    private bool thereIsAPlayer = false;//variable for check if the player CI SEGUE!!!!


    private void Awake()
    {
        damage = archerDamage;
        attackDelay = archerAttackDelay;
        delay = archerAttackDelay;

        hitColliders = new Collider2D[maxArray];
    }

    private void FixedUpdate()
    {
        
        if(player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        //Debug.Log("Me update");
        //Debug.Log(delay);
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
        //Debug.Log("To search");
        enemyState = EnemyState.searchPlayer;

    }

    public override void Attack()
    {
        //Debug.Log("damaging");
        /*
        hitColliders[i].gameObject.SendMessage("Damage", damage);
        i = 0;
        delay = attackDelay;
        System.Array.Clear(hitColliders, 0, maxArray);*/

        //create and shoot arrow
        Instantiate(arrowPrefab, gameObject.transform.position, Quaternion.identity);
        /*
        arrow.transform.position = transform.position;

        Vector3 playerPos = player.transform.position;
        Vector3 playerPosFlattened = new Vector3(playerPos.x, playerPos.y, 0);
        arrow.transform.LookAt(playerPosFlattened);*/

        enemyState = EnemyState.idle;
        delay = archerAttackDelay;
    }

    public override void SearchPlayer()
    {
        //check if the player is in the area
        System.Array.Clear(hitColliders, 0, maxArray);
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

        i = 0;

        System.Array.Clear(hitColliders, 0, maxArray);
        //run away if the player is too close
        Physics2D.OverlapCircle(transform.position, escapeArea, contactFilter, hitColliders);
        foreach (Collider2D collider in hitColliders)
        {
            if (hitColliders[i] != null)
            {
                //Debug.Log("Hitted" + i);
                if (hitColliders[i].CompareTag("Player"))
                {
                    enemyState = EnemyState.escape;
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

    public override void Escape()
    {
        Debug.Log("Ruun forrest, ruuuuuuuuuuuuuuunnnnnnnnnnnn!!!!!!");
        i = 0;
        System.Array.Clear(hitColliders, 0, maxArray);
        Physics2D.OverlapCircle(transform.position, escapeArea, contactFilter, hitColliders);
        foreach (Collider2D collider in hitColliders)
        {
            if (hitColliders[i] != null)
            {
                //Debug.Log("Hitted" + i);
                if (hitColliders[i].CompareTag("Player"))
                {
                    direction = gameObject.transform.position.x - player.transform.position.x;

                    movement = new Vector2(Mathf.Sign(direction) * speed * Time.deltaTime, 0);
                    gameObject.transform.Translate(movement);
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
        System.Array.Clear(hitColliders, 0, maxArray);

        Physics2D.OverlapCircle(transform.position, escapeArea, contactFilter, hitColliders);
        thereIsAPlayer = false;
        foreach (Collider2D collider in hitColliders)
        {
            if (hitColliders[i] != null)
            {
                //Debug.Log("Hitted" + i);
                if (hitColliders[i].CompareTag("Player"))
                {
                    thereIsAPlayer = true;
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

            if(!thereIsAPlayer)
            {
                enemyState = EnemyState.idle;
            }

        }
    }
}

