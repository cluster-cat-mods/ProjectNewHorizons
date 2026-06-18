using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> enemySpawnPositions;
    [SerializeField] private Vector2 offsetOnSpawnPositionXZ;
    [SerializeField] private GameObject debugEnemy;
    void Start()
    {
        var SpawnPoints = GameObject.FindGameObjectsWithTag("SpawnNode");
        foreach (GameObject o in SpawnPoints)
        {
            if (enemySpawnPositions.Contains(o.transform)) continue;
            enemySpawnPositions.Add(o.transform);
        }   
    }
    public IEnumerator StartWave(EnemyWave[] enemyWaves, int wave)
    {
        var enemyCounter = 0;
        for (int i = 0; i < enemyWaves[wave].enemyGroups.Length; i++)
        {
            enemyCounter += enemyWaves[wave].enemyGroups[i].enemyCount;
        }
        float spawnDelay = enemyWaves[wave].waveDuration / enemyCounter;
        Debug.Log($"wave duration: {enemyWaves[wave].waveDuration} / enemy count: {enemyCounter} =  spawndelay: {spawnDelay}");

        yield return StartCoroutine(SpawnEnemy(spawnDelay, enemyCounter, enemyWaves, wave));
    }

    [Button]
    private void SpawnDebugEnemy()
    {
        var randomSpawnpointIndex = Random.Range(0, enemySpawnPositions.Count);
        var spawnPosition = enemySpawnPositions[randomSpawnpointIndex].position + new Vector3(Random.Range(-offsetOnSpawnPositionXZ.x, offsetOnSpawnPositionXZ.x), 0, Random.Range(-offsetOnSpawnPositionXZ.y, offsetOnSpawnPositionXZ.y));
        Instantiate(debugEnemy, spawnPosition, Quaternion.identity, transform);
    }
    private IEnumerator SpawnEnemy(float spawnDelay, int totalEnemyCount, EnemyWave[] enemyWaves, int wave)
    {
        //Debug.Log($"total enemy count = {totalEnemyCount}");
        while (totalEnemyCount > 0)
        {
            yield return new WaitForSeconds(spawnDelay);

            var randomEnemyGroupIndex = PickEnemyGroup(enemyWaves, wave);
            var enemy = enemyWaves[wave].enemyGroups[randomEnemyGroupIndex].enemy;
            var randomSpawnpointIndex = Random.Range(0, enemySpawnPositions.Count);
            var spawnPosition = enemySpawnPositions[randomSpawnpointIndex].position + new Vector3(Random.Range(-offsetOnSpawnPositionXZ.x, offsetOnSpawnPositionXZ.x), 0, Random.Range(-offsetOnSpawnPositionXZ.y, offsetOnSpawnPositionXZ.y));
            /*Enemy enemy =*/ Instantiate(enemy, spawnPosition, Quaternion.identity, transform);
            //enemy.SetSpawnPoint(transform);
            enemyWaves[wave].enemyGroups[randomEnemyGroupIndex].enemyCount--;
            totalEnemyCount--;
            //Debug.Log($"random spawn index = {randomSpawnpointIndex} that is at {enemySpawnPositions[randomSpawnpointIndex].position}");
            //Debug.Log($"enemy group {randomEnemyGroupIndex} has {enemyWaves[wave].enemyGroups[randomEnemyGroupIndex].enemyCount} enemies to spawn left");
            //Debug.Log($"total enemy Count = {totalEnemyCount}");
        }
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
