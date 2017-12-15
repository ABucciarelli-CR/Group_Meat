using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherArenaDoor : MonoBehaviour 
{

	public GameObject phisicalDoor;

	private GameObject gameManager;
	private GlobalVariables globalVariables;


	// Use this for initialization
	void Start () 
	{
		gameManager = GameObject.Find ("GameManager");
		globalVariables = gameManager.GetComponent<GlobalVariables> ();

		phisicalDoor.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (globalVariables.closeDoor)
		{
			phisicalDoor.SetActive (true);
		} 
		else
		{
			phisicalDoor.SetActive (false);
		}
	}
}
