using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarcryAOE : MonoBehaviour
{
    private float doMassiveDamage = 500000f;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "enemy")
        {
            collision.SendMessage("Damage", doMassiveDamage);
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "enemy")
        {
            collision.SendMessage("Damage", doMassiveDamage);
        }
    }
}
