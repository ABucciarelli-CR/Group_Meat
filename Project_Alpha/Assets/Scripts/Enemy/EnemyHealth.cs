using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    /*[HideInInspector]*/public int health;
    [HideInInspector]public int maxHealth;
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
        //minDepth = -Mathf.Infinity;
        //maxDepth = Mathf.Infinity;

        spriteRenderer = GetComponent<SpriteRenderer>();

        enemyDamagedColor = Color.red;
        enemyStandardColor = Color.white;

        playerAndShieldsLayerMask = (1 << LayerMask.NameToLayer("player")) | (1 << LayerMask.NameToLayer("blockPlayer"));
    }

    public void Damage(int dmg)
    {
        //ceckPlayerLeftRayHit2D = Physics2D.Raycast(new Vector2(0,0), Vector2.left, maxDistance, playerAndShieldsLayerMask.value);
        //ceckPlayerRightRayHit2D = Physics2D.Raycast(new Vector2(0, 0), -Vector2.left, maxDistance, playerAndShieldsLayerMask.value);
        
        hitLeft = Physics2D.Raycast(transform.position, Vector2.left, maxDistance, playerAndShieldsLayerMask);
        hitRight = Physics2D.Raycast(transform.position, -Vector2.left, maxDistance, playerAndShieldsLayerMask);

        //Debug.Log(hitRight.collider.name);
        //Debug.Log(hitLeft.collider.name);
        //Debug.DrawRay(transform.position, Vector2.left, Color.red);
        //Debug.DrawRay(transform.position, -Vector2.left, Color.red);

        //if (ceckPlayerRightRayHit2D.collider != null || ceckPlayerLeftRayHit2D.collider != null)
        if (hitLeft.collider != null || hitRight.collider != null)
        {
            //Debug.Log("Inside");

            //if (ceckPlayerRightRayHit2D.collider.CompareTag("player") || ceckPlayerLeftRayHit2D.collider.CompareTag("player"))
            //Debug.Log(hitRight.collider.tag);
            //Debug.Log(hitLeft.collider.tag);

            if(hitLeft.collider != null)
            {
                //Debug.Log("Here1!");
                if (hitLeft.collider.CompareTag("Player"))
                {
                    spriteRenderer.color = enemyDamagedColor;
                    health -= dmg;
                    //Debug.Log("Damaged");
                }
            }

            if (hitRight.collider != null)
            {
                //Debug.Log("Here1!");
                if (hitRight.collider.CompareTag("Player"))
                {
                    spriteRenderer.color = enemyDamagedColor;
                    health -= dmg;
                    Debug.Log("Damaged");
                }
            }
        }

    }
    /*
    private void FixedUpdate()
    {
        ceckPlayerLeftRayHit2D = Physics2D.Raycast(new Vector2(0, 0), Vector2.left, maxDistance, playerAndShieldsLayerMask.value);
        ceckPlayerRightRayHit2D = Physics2D.Raycast(new Vector2(0, 0), -Vector2.left, maxDistance, playerAndShieldsLayerMask.value);

        Debug.DrawRay(new Vector2(0, 0), ceckPlayerRightRayHit2D.point, Color.green);
        if (ceckPlayerRightRayHit2D.collider != null || ceckPlayerLeftRayHit2D.collider != null)
        {
            Debug.Log("Inside");

            if (ceckPlayerRightRayHit2D.collider.CompareTag("player") || ceckPlayerLeftRayHit2D.collider.CompareTag("player"))
            {
                health -= 5;
                Debug.Log("Damaged");
            }
        }
    }*/
        
    public void SetHealth(int settedHealth)
    {
        health = settedHealth;
        maxHealth = settedHealth;
    }

    public void Heal(int heal)
    {
        health = heal;
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

        if (health <= 0)
        {
            Debug.Log("DEAD");
        }

    }
}

