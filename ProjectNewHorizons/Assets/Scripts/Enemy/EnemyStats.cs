using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public EnemyStartStats startStats;
}

[System.Serializable]
public struct EnemyStartStats
{
    public bool canSpawnEnemy;
    public bool bossEnemy;

    //normal stats
    [AllowNesting] public int hp;
    [AllowNesting] public int damage;
    [AllowNesting] public float speed;

    [Space(30)]

    //Enemy spawning enemy
    [ShowIf("canSpawnEnemy"), AllowNesting] public GameObject enemy;
    [ShowIf("canSpawnEnemy"), AllowNesting] public int enemySpawnCount;

    [Space(30)]

    //boss enemy
    [ShowIf("bossEnemy"), AllowNesting] public int maxHp;
}
