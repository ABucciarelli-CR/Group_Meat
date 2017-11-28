using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDead : MonoBehaviour 
{

	private SpriteRenderer sr;
	private Color green;

	// Use this for initialization
	void Start () 
	{
		sr = GetComponent<SpriteRenderer>();

		green = new Color(0, 1, 0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(gameObject.CompareTag("UnEatable"))
		{
			sr.color = green;
		}
	}
}
