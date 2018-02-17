using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveFrenzy : MonoBehaviour
{
    private GlobalVariables globalVariables;

    private void Awake()
    {
        globalVariables = GameObject.Find("GameManager").GetComponent<GlobalVariables>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            globalVariables.frenzyCanBeUsed = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            globalVariables.frenzyCanBeUsed = true;
        }
    }
}
