using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipVisualizedButtons : MonoBehaviour
{
    /*[HideInInspector]*/public PlayerStateMachine plyrSttMcn;
    private bool facingRight = true;

    private void Awake()
    {
        if(gameObject.GetComponent<MeshRenderer>() != null)
        {
            gameObject.GetComponent<MeshRenderer>().sortingOrder = 4;
        }
    }

    void Update ()
    {
		if(plyrSttMcn.facingRight != facingRight)
        {
            facingRight = !facingRight;
            Vector3 normalScale = transform.localScale;
            normalScale.x *= -1;
            transform.localScale = normalScale;
        }
	}
}
