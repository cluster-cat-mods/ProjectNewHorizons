using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [SerializeField] private EnemyStartStats startStats;
    public EnemyStartStats BaseStats => startStats;
}

[System.Serializable]
public struct EnemyStartStats
{
    public bool canSpawnEnemy;
    public bool bossEnemy;

    //normal runtimeStats
    [AllowNesting] public int hp;
    [AllowNesting] public int damage;
    [AllowNesting] public float speed;
    [AllowNesting] public int coinBounty;
    [AllowNesting] public string spawnSoundPath;
    [AllowNesting] public string deathSoundPath;
    [AllowNesting] public string hitSoundPath;

    [Space(30)]

    //Enemy spawning enemy
    [ShowIf("canSpawnEnemy"), AllowNesting] public GameObject enemy;
    [ShowIf("canSpawnEnemy"), AllowNesting] public int enemySpawnCount;
    [ShowIf("canSpawnEnemy"), AllowNesting] public float spawnDelay;

    [Space(30)]

    //boss enemy
    [ShowIf("bossEnemy"), AllowNesting] public int maxHp;
}
