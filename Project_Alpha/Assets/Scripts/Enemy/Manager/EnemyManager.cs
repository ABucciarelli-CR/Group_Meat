using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> enemy = new List<GameObject>();

    public void Add(GameObject thisEnemy)
    {
        enemy.Add(thisEnemy);
    }

    public void Remove(GameObject thisEnemy)
    {
        enemy.Remove(thisEnemy);
    }
}
