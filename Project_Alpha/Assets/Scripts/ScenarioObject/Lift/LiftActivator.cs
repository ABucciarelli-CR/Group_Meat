using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftActivator : MonoBehaviour
{
    public GameObject lift;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        lift.SendMessage("ActiveLift", true);
        Destroy(this.gameObject);
    }
}
