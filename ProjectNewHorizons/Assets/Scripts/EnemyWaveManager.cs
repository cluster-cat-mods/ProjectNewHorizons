using NaughtyAttributes;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField] private GameManager manager;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private EnemyWave[] enemyWaves;

    [SerializeField] private float waveDelay;
    private bool _skipWaveDelay = false;

    private void Start()
    {
        if (manager == null)
        {
            manager = FindAnyObjectByType<GameManager>();
        }

        if (enemySpawner == null)
        {
            enemySpawner = FindAnyObjectByType<EnemySpawner>();
        }

        // Do this when initializing your enemyWaves array/list at Start()
        for (int i = 0; i < enemyWaves.Length; i++)
        {
            // Creates a unique runtime-only duplicate of the Scriptable Object
            enemyWaves[i] = Instantiate(enemyWaves[i]);
        }

    }

    [Button]
    public IEnumerator StartRun()
    {
        foreach (EnemyWave wave in enemyWaves)
        {
            yield return StartCoroutine(StartEnemySpawners());
            _skipWaveDelay = false;

            float timer = 0f;

            while (timer < waveDelay && !_skipWaveDelay)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            manager.IncreaseWave();
        }
    }

    [Button]
    public void SkipWaveDelay()
    {
        _skipWaveDelay = true;
    }

    [Button]
    public IEnumerator StartEnemySpawners()
    {
        yield return StartCoroutine(enemySpawner.StartWave(enemyWaves, manager.wave));
    }
}
