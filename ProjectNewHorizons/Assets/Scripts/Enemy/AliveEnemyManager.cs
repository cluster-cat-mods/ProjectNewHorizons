using System.Collections.Generic;
using UnityEngine;

public class AliveEnemyManager : MonoBehaviour
{
    private List<GameObject> _aliveEnemyList = new();
    public List<GameObject> AliveEnemies()
    {
        return _aliveEnemyList;
    }
    public int AliveEnemiesCount()
    {
        return _aliveEnemyList.Count;
    }

    public void AddEnemy(GameObject enemy)
    {
        if (_aliveEnemyList.Contains(enemy))
            return;

        _aliveEnemyList.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy) 
    {
        _aliveEnemyList.Remove(enemy);
        Destroy(enemy);
    }
}
