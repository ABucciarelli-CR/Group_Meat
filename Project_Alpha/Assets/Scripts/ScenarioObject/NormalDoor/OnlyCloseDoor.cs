using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyCloseDoor : MonoBehaviour
{
    public GameObject door;
    public LayerMask playerLayerMask;
    public bool becomeVisible = true;
    public bool thisDoorClosed = false;
    
    private RaycastHit2D hitRight;
    private float maxDistance = 5;

    void Start()
    {
        hitRight = new RaycastHit2D();
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        door.SetActive(false);
        playerLayerMask = (1 << LayerMask.NameToLayer("player")) | (1 << LayerMask.NameToLayer("midGhost"));
    }

    private void Update()
    {
        //Debug.Log(playerLayerMask.value);
        hitRight = Physics2D.Raycast(transform.position, -Vector2.left, maxDistance, playerLayerMask);

        if (hitRight.collider != null)
        {
            //Debug.Log("Here!1");
            if (hitRight.collider.CompareTag("Player"))
            {
                door.SetActive(true);
                if (!becomeVisible)
                {
                    door.GetComponent<SpriteRenderer>().enabled = false;
                }
                gameObject.SetActive(false);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            door.SetActive(true);
            if (!becomeVisible)
            {
                door.GetComponent<SpriteRenderer>().enabled = false;
            }
            gameObject.SetActive(false);
        }
    }
}
