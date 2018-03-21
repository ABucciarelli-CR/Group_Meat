using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerSpawnerArena : MonoBehaviour
{
    public List<GameObject> arenaSpawners;
    public List<GameObject> myHealer;

    private bool endArenaEnemy = false;

	void Update ()
    {
        endArenaEnemy = true;
        foreach (GameObject go in arenaSpawners)
        {
            if(!go.GetComponent<EnemySpawnerStateMachine>().endSpawn)
            {
                endArenaEnemy = false;
                break;
            }
        }

        if(endArenaEnemy)
        {
            foreach(GameObject go in myHealer)
            {
                go.GetComponent<EnemyHealer>().callForTheEnd = true;
            }
        }
	}
}
