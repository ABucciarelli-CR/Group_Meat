using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheArenaDoor;

public class FinalDialogActivator : MonoBehaviour
{
    public List<GameObject> dialogs;
    public GameObject arenaDoor;
	void Update () {}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogs[0].GetComponent<TextActivator>().SendMessage("Activation");
            arenaDoor.GetComponent<ArenaDoorExitCollider>().isPlayer = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogs[0].GetComponent<TextActivator>().SendMessage("Activation");
            arenaDoor.GetComponent<ArenaDoorExitCollider>().isPlayer = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
