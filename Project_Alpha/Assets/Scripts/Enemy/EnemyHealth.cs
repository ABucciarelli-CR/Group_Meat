using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 50;

	public void Damage(int dmg)
    {
        health -= dmg;
        Debug.Log("Damaged");
    }
	
	void Update ()
    {
        if (health <= 0)
        {
            Debug.Log("DEAD");
        }
	}
}
