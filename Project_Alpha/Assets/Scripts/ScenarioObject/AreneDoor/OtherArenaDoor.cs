using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherArenaDoor : MonoBehaviour 
{

	public GameObject phisicalDoor;

	private GlobalVariables globalVariables;


	// Use this for initialization
	void Start () 
	{
		globalVariables = GameObject.Find("GameManager").GetComponent<GlobalVariables> ();

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
