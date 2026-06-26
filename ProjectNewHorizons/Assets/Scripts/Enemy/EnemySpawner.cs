using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> enemySpawnPositions;
    [SerializeField] private Vector2 offsetOnSpawnPositionXZ;
    [SerializeField] private GameObject debugEnemy;

    [SerializeField] private List<Transform> openPathsList = new();

    private int _totalEnemyCount;
    void Start()
    {
        FindSpawnPositions();
    }
    
    public IEnumerator StartWave(EnemyWave[] enemyWaves, int wave)
    {
        for (int i = 0; i < enemyWaves[wave].enemyGroups.Length; i++)
        {
            _totalEnemyCount += enemyWaves[wave].enemyGroups[i].enemyCount;
        }
        float spawnDelay = enemyWaves[wave].waveDuration / _totalEnemyCount;
        //Debug.Log($"wave duration: {enemyWaves[wave].waveDuration} / enemy count: {_totalEnemyCount} =  spawndelay: {spawnDelay}");

        foreach (int i in enemyWaves[wave].openPaths)
        {
            openPathsList.Add(enemySpawnPositions[i]);
        }
        yield return StartCoroutine(SpawnCoroutine(spawnDelay, enemyWaves, wave));
        openPathsList.Clear();
    }

    [Button]
    private void FindSpawnPositions()
    {
        var spawnPoints = GameObject.FindGameObjectsWithTag("SpawnNode");
        foreach (GameObject o in spawnPoints)
        {
            if (enemySpawnPositions.Contains(o.transform)) continue;
            enemySpawnPositions.Add(o.transform);
        }   
    }

    [Button]
    private void SpawnDebugEnemy()
    {
        int randomSpawnpointIndex = Random.Range(0, enemySpawnPositions.Count);
        Vector3 spawnPosition = enemySpawnPositions[randomSpawnpointIndex].position;
        Enemy enemyScript = Instantiate(debugEnemy, spawnPosition, Quaternion.identity, transform).GetComponent<Enemy>();
        enemyScript.SetStuff(enemySpawnPositions[randomSpawnpointIndex]);
    }
    private IEnumerator SpawnCoroutine(float spawnDelay, EnemyWave[] enemyWaves, int wave)
    {
        //Debug.Log($"total enemy count = {totalEnemyCount}");
        while (_totalEnemyCount > 0)
        {
            yield return new WaitForSeconds(spawnDelay);
            SpawnEnemy(enemyWaves, wave);
        }
    }

    private void SpawnEnemy(EnemyWave[] enemyWaves, int wave)
    {
        var randomEnemyGroupIndex = PickEnemyGroup(enemyWaves, wave);
        var enemy = enemyWaves[wave].enemyGroups[randomEnemyGroupIndex].enemy;

        var randomSpawnpointIndex = Random.Range(0, openPathsList.Count);
        var spawnPosition = openPathsList[randomSpawnpointIndex].position/* + new Vector3(Random.Range(-offsetOnSpawnPositionXZ.x, offsetOnSpawnPositionXZ.x), 0, Random.Range(-offsetOnSpawnPositionXZ.y, offsetOnSpawnPositionXZ.y))*/;
        
        Enemy enemyScript = Instantiate(enemy, spawnPosition, Quaternion.identity, transform).GetComponent<Enemy>();
        //Debug.Log(enemyScript);
        //Debug.Log(enemyScript.gameObject.transform);
        enemyScript.SetStuff(openPathsList[randomSpawnpointIndex]);
        enemyWaves[wave].enemyGroups[randomEnemyGroupIndex].enemyCount--;
        _totalEnemyCount--;
        //Debug.Log($"random spawn index = {randomSpawnpointIndex} that is at {enemySpawnPositions[randomSpawnpointIndex].position}");
        //Debug.Log($"enemy group {randomEnemyGroupIndex} has {enemyWaves[wave].enemyGroups[randomEnemyGroupIndex].enemyCount} enemies to spawn left");
        //Debug.Log($"total enemy Count = {_totalEnemyCount}");
    }
    private int PickEnemyGroup(EnemyWave[] enemyWaves, int wave)
    {
        var randomEnemyGroupIndex = Random.Range(0, enemyWaves[wave].enemyGroups.Length);

        while (enemyWaves[wave].enemyGroups[randomEnemyGroupIndex].enemyCount <= 0)
        {
            randomEnemyGroupIndex = Random.Range(0, enemyWaves[wave].enemyGroups.Length);
        }

        return randomEnemyGroupIndex;
    }
}
