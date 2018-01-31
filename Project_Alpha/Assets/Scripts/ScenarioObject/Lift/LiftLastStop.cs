using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftLastStop : MonoBehaviour
{
    public GameObject lift;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        lift.SendMessage("ActiveLift", false);
    }
}
