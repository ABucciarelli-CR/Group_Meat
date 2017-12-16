using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlankCollider : MonoBehaviour
{
	

	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		StartCoroutine (Flank ());
	}

	IEnumerator Flank()
	{
		//print ("ciao");
		yield return new WaitForSeconds(1.5f);
		transform.parent.GetComponent<EnemyTank> ().facingleft = false;
	}
}
