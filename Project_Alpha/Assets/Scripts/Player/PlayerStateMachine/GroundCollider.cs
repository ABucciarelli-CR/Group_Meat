using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollider : MonoBehaviour
{
    public GameObject thisPlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collisionEnter");
        thisPlayer.SendMessage("GoJump", true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("collisionEnter");
        thisPlayer.SendMessage("GoJump", true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("collisionExit");
        thisPlayer.SendMessage("GoJump", false);
    }
}
