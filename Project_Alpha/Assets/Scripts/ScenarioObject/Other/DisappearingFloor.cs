using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingFloor : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Healer" || collision.name == "Healer(Clone)")
        {
            collision.GetComponent<EnemyHealer>().disappearingPlatform = gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Healer" || collision.name == "Healer(Clone)")
        {
            collision.GetComponent<EnemyHealer>().disappearingPlatform = gameObject;
        }
    }
}
