using System.Collections;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "EnemyWave", menuName = "Scriptable Objects/EnemyWave")]
public class EnemyWave : ScriptableObject
{
    public EnemyGroup[] enemyGroups;
    public float waveDuration;
    public int[] openPaths;
}
