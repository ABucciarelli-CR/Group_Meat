using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAttackArea : MonoBehaviour
{
    public GameObject thisEnemy;
    //public GameObject thisCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision);
        thisEnemy.SendMessage("IsPlayerdamageable", true);
        //thisCollider.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("IsQuit");
        thisEnemy.SendMessage("IsPlayerdamageable", false);
        //thisCollider.SetActive(true);
    }
}
