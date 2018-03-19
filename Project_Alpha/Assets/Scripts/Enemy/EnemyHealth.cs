using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    /*[HideInInspector]*/public int health;
    [HideInInspector]public int maxHealth;
    /*[HideInInspector]*/public GameObject bloodDamaged;

    public LayerMask playerAndShieldsLayerMask;

    private float maxDistance;
    //private float minDepth;
    //private float maxDepth;
    private SpriteRenderer spriteRenderer;
    private Color enemyDamagedColor;
    private Color enemyStandardColor;

    private int waitFrame = 2;
    private int waitedFrame = 0;

    private RaycastHit2D hitLeft;
    private RaycastHit2D hitRight;

    private void Awake()
    {
        hitLeft = new RaycastHit2D();
        hitRight = new RaycastHit2D();
        maxDistance = Mathf.Infinity;

        spriteRenderer = GetComponent<SpriteRenderer>();

        enemyDamagedColor = Color.red;
        enemyStandardColor = Color.white;

        playerAndShieldsLayerMask = (1 << LayerMask.NameToLayer("player")) | (1 << LayerMask.NameToLayer("blockPlayer"));
    }

    public void Damage(int dmg)
    {
        hitLeft = Physics2D.Raycast(transform.position, Vector2.left, maxDistance, playerAndShieldsLayerMask);
        hitRight = Physics2D.Raycast(transform.position, -Vector2.left, maxDistance, playerAndShieldsLayerMask);
        
        if (hitLeft.collider != null || hitRight.collider != null)
        {

            if(hitLeft.collider != null)
            {
                //Debug.Log("Here1!");
                if (hitLeft.collider.CompareTag("Player"))
                {
                    //damaged animation
                    DoInstantiate(bloodDamaged, 3, 3);

                    spriteRenderer.color = enemyDamagedColor;
                    health -= dmg;
                    //Debug.Log("Damaged");
                }
            }

            if (hitRight.collider != null)
            {
                //Debug.Log("Here2!");
                if (hitRight.collider.CompareTag("Player"))
                {
                    //damaged animation
                    DoInstantiate(bloodDamaged, 3, 3);

                    spriteRenderer.color = enemyDamagedColor;
                    health -= dmg;
                    //Debug.Log("Damaged");
                }
            }
        }

    }
     
    public void SetHealth(int settedHealth)
    {
        health = settedHealth;
        maxHealth = settedHealth;
    }

    public void Heal(int heal)
    {
        health = Mathf.RoundToInt(heal / 2);
    }

    public void HealIfAlive(int heal)
    {
        if(health > 0 && (health + heal) < maxHealth)
        {
            health += heal;
        }
        if((health + heal) >= maxHealth)
        {
            health = maxHealth;
        }
    }

    public void DoInstantiate(GameObject obj, float x, float y)
    {
        GameObject objPlaceholder = Instantiate(obj, this.transform.position + new Vector3(0, 30, 0), Quaternion.identity, this.gameObject.transform);
        objPlaceholder.gameObject.transform.localScale = new Vector3(x, y, 0);
    }

    void Update()
    {
        if (spriteRenderer.color == enemyDamagedColor && waitedFrame == waitFrame)
        {
            waitedFrame = 0;
            spriteRenderer.color = enemyStandardColor;
        }
        if(spriteRenderer.color == enemyDamagedColor)
        {
            waitedFrame++;
        }


        /*
        if (health <= 0)
        {
            Debug.Log("DEAD");
        }*/

    }
}

