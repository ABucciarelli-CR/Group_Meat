using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWay : MonoBehaviour {
    public GameObject door;
    public LayerMask playerLayerMask;
    public bool thisDoorClosed = false;
    private RaycastHit2D hitRight;
    private float maxDistance = 5;
    // Use this for initialization
    void Start () {
        hitRight = new RaycastHit2D();
        playerLayerMask = (1 << LayerMask.NameToLayer("player")) | (1 << LayerMask.NameToLayer("midGhost"));
    }
	
	// Update is called once per frame
	void Update () {
        hitRight = Physics2D.Raycast(transform.position, -Vector2.left, maxDistance, playerLayerMask);

        if (hitRight.collider != null)
        {
            //Debug.Log("Here!1");
            if (hitRight.collider.CompareTag("Player"))
            {
                thisDoorClosed = true;
            }
        }
    }
}
