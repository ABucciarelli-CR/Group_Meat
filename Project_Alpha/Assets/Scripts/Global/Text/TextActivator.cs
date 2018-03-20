using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextActivator : MonoBehaviour
{
    private void Update() { }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.GetComponent<TextAppear>().SendMessage("StartText");
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.GetComponent<TextAppear>().SendMessage("StartText");
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
