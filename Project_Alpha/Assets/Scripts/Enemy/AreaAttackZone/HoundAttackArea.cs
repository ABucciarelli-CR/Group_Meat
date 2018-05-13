using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoundAttackArea : MonoBehaviour
{

    public GameObject thisEnemy;
    public GameObject thisCircleCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("IsIn");
        thisEnemy.SendMessage("IsPlayerdamageable", true);
        thisCircleCollider.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("IsQuit");
        thisEnemy.SendMessage("IsPlayerdamageable", false);
        thisCircleCollider.SetActive(true);
    }
}
