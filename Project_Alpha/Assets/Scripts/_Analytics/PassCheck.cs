using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class PassCheck : MonoBehaviour
{
    AnalyticsTracker anTracker;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        anTracker.TriggerEvent();
        gameObject.SetActive(false);
    }
}
