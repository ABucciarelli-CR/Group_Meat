using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class ActionIfQuit : MonoBehaviour
{
    public AnalyticsTracker anTracker;

    private void OnApplicationQuit()
    {
        anTracker.TriggerEvent();
    }
}
