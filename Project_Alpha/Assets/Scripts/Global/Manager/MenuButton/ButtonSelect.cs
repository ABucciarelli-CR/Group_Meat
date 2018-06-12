using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSelect : MonoBehaviour
{
    public GameObject button;

	void Update ()
    {
        if (Input.GetButtonDown("Attack"))
        {
            if(button.GetComponent<Cambio_scena>() != null)
            {
                button.GetComponent<Cambio_scena>().ChangeScene();
                //button.GetComponent<Button>()
            }
        }
	}
}
