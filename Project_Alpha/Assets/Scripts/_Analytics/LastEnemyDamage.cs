using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class LastEnemyDamage : MonoBehaviour
{
    public string enemyName;
    public AnalyticsTracker anTracker;

    public void KilledBy(string thisEnemyName)
    {
        enemyName = thisEnemyName;
        Debug.Log("Killed by: " + enemyName);
        anTracker.TriggerEvent();
    }
}
