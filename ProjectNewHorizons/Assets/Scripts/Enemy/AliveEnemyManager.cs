using System.Collections.Generic;
using UnityEngine;

public class AliveEnemyManager : MonoBehaviour
{
    private List<GameObject> _aliveEnemyList = new();
    private List<Transform> _aliveEnemyTransformList = new();
    public List<GameObject> AliveEnemies()
    {
        return _aliveEnemyList;
    }
    public List<Transform> AliveEnemyTransforms()
    {
        return _aliveEnemyTransformList;
    }
    public int AliveEnemiesCount()
    {
        return _aliveEnemyList.Count;
    }

    public void AddEnemy(GameObject enemy)
    {
        _aliveEnemyList.Add(enemy);
        _aliveEnemyTransformList.Add(enemy.transform);
    }

    public void RemoveEnemy(GameObject enemy) 
    {
        _aliveEnemyList.Remove(enemy);
        _aliveEnemyTransformList.Remove(enemy.transform);
    }
}
