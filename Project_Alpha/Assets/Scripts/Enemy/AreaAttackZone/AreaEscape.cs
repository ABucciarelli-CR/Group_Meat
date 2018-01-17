using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEscape : MonoBehaviour
{
    public GameObject thisEnemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision);
        thisEnemy.SendMessage("IsPlayerTooNear", true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("IsQuit");
        thisEnemy.SendMessage("IsPlayerTooNear", false);
    }
}
