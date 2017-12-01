using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 50;
    public LayerMask playerAndShieldsLayerMask;

    private float maxDistance;
    //private float minDepth;
    //private float maxDepth;


    private RaycastHit2D hitLeft;
    private RaycastHit2D hitRight;

    private void Awake()
    {
        hitLeft = new RaycastHit2D();
        hitRight = new RaycastHit2D();
        maxDistance = Mathf.Infinity;
        //minDepth = -Mathf.Infinity;
        //maxDepth = Mathf.Infinity;

        playerAndShieldsLayerMask = (1 << LayerMask.NameToLayer("player")) | (1 << LayerMask.NameToLayer("blockPlayer"));
    }

    public void Damage(int dmg)
    {
        //ceckPlayerLeftRayHit2D = Physics2D.Raycast(new Vector2(0,0), Vector2.left, maxDistance, playerAndShieldsLayerMask.value);
        //ceckPlayerRightRayHit2D = Physics2D.Raycast(new Vector2(0, 0), -Vector2.left, maxDistance, playerAndShieldsLayerMask.value);

        hitLeft = Physics2D.Raycast(transform.position, Vector2.left, maxDistance, playerAndShieldsLayerMask);
        hitRight = Physics2D.Raycast(transform.position, -Vector2.left, maxDistance, playerAndShieldsLayerMask);

        //if (ceckPlayerRightRayHit2D.collider != null || ceckPlayerLeftRayHit2D.collider != null)
        if (hitLeft.collider != null || hitRight != null)
        {
            //Debug.Log("Inside");

            //if (ceckPlayerRightRayHit2D.collider.CompareTag("player") || ceckPlayerLeftRayHit2D.collider.CompareTag("player"))
            Debug.Log(hitLeft.collider.tag);
            if (hitLeft.collider.CompareTag("Player") || hitRight.collider.CompareTag("Player"))
            {
                health -= dmg;
                //Debug.Log("Damaged");
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
        
    void Update()
    {
        if (health <= 0)
        {
            Debug.Log("DEAD");
        }
    }
}

