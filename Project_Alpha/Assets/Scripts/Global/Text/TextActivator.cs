using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextActivator : MonoBehaviour
{
    public List<GameObject> dialogs;
    public bool activeItself = false;

    private void Update()
    {
        if(activeItself)
        {
            for(int i=0; i<dialogs.Capacity;i++)
            {
                if(!dialogs[i].GetComponent<TextAppear>().textInDoing)
                {
                    dialogs[i].GetComponent<TextAppear>().SendMessage("StartText");
                    Debug.Log("___________________________atchung__________________");
                }
                Debug.Log("this Dialog _____________________" + dialogs[i].name + " " + i);
                Debug.Log("first" + dialogs[i].GetComponent<TextAppear>().appearingText.Length);
                Debug.Log("second" + dialogs[i].GetComponent<TextAppear>().text.text.Length);
                /*
                else if(dialogs[i].GetComponent<TextAppear>().appearingText.Length == dialogs[i].GetComponent<TextAppear>().text.text.Length)
                {
                    i++;
                }*/
            }
        }
    }

    private void Activation()
    {
        activeItself = true;
    }

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
