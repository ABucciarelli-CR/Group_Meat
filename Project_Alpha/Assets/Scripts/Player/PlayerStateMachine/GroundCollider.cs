using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollider : MonoBehaviour
{
    public GameObject thisPlayer;
    private bool canjump = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(canjump)
        {
            StartCoroutine(Wait());
            thisPlayer.SendMessage("GoJump", true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (canjump)
        {
            StartCoroutine(Wait());
            thisPlayer.SendMessage("GoJump", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (canjump)
        {
            StartCoroutine(Wait());
            thisPlayer.SendMessage("GoJump", true);
        }
    }

    IEnumerator Wait()
    {
        canjump = false;
        yield return new WaitForSeconds(.5f);
        canjump = true;
    }
}
