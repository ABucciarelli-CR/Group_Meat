using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerActivator : MonoBehaviour
{

    public GameObject[] spawner;
    public List<GameObject> enemy;
    public GameObject lift;

    private bool activation = false;
    private bool startCheck = false;
    private bool startLift = false;

	void Start ()
    {
		
	}
	
	void Update ()
    {
		if(activation)
        {
            lift.SendMessage("ActiveLift", false);

            foreach(GameObject spawn in spawner)
            {
                spawn.SendMessage("StartToSpawn", true);
                spawn.SendMessage("AddEnemy", enemy);
            }

            this.GetComponent<Collider2D>().enabled = false;
            activation = false;
        }

        foreach(GameObject enemies in enemy)
        {
            enemies.GetComponent<EnemyTank>().SendMessage("AddToList", enemy);
            Debug.Log(enemies.name);
        }

        if(enemy.Count == 0 && startCheck)
        {
            lift.SendMessage("ActiveLift", true);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Lift"))
        {
            activation = true;
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        startCheck = true;
    }
}
