using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameManager manager;
    [SerializeField] private Transform[] EnemySpawnPositions;
    void Start()
    {
        if (manager == null)
        {
            manager = FindAnyObjectByType<GameManager>();
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
        Debug.Log($"{enemyWaves[wave].waveDuration} / {enemyCounter} = {spawnDelay}");

        yield return StartCoroutine(SpawnEnemy(spawnDelay, enemyCounter, enemyWaves, wave));
    }

    private IEnumerator SpawnEnemy(float spawnDelay, int totalEnemyCount, EnemyWave[] enemyWaves, int wave)
    {
        //Debug.Log($"total enemy count = {totalEnemyCount}");
        while (totalEnemyCount > 0)
        {
            yield return new WaitForSeconds(spawnDelay);
            var randomEnemyGroupIndex = PickEnemyGroup(enemyWaves, wave);
            var enemy = enemyWaves[wave].enemyGroups[randomEnemyGroupIndex].Enemy;
            var randomSpawnpointIndex = Random.Range(0, EnemySpawnPositions.Length);

            Instantiate(enemy, EnemySpawnPositions[randomSpawnpointIndex].position, Quaternion.identity, transform);
            enemyWaves[wave].enemyGroups[randomEnemyGroupIndex].enemyCount--;
            totalEnemyCount--;
            //Debug.Log($"random spawn index = {randomSpawnpointIndex} that is at {EnemySpawnPositions[randomSpawnpointIndex].position}");
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
