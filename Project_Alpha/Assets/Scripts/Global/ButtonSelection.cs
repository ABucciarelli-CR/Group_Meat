using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelection : MonoBehaviour
{
    public EventSystem eventSystem;
    public GameObject selectedObject;

    private bool buttonSelected;
    
	
	void Update ()
    {
		if(Input.GetAxisRaw("Horizontal") != 0 && !buttonSelected)
        {
            eventSystem.SetSelectedGameObject(selectedObject);
        }
	}

    private void OnDisable()
    {
        buttonSelected = false;
    }
}
