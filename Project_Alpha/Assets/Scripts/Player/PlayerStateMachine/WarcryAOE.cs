using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarcryAOE : MonoBehaviour
{
    public bool activeWarcry = false;
    private float doMassiveDamage = 500000f;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" && activeWarcry)
        {
            collision.SendMessage("Damage", doMassiveDamage);
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && activeWarcry)
        {
            collision.SendMessage("Damage", doMassiveDamage);
        }
    }
}
