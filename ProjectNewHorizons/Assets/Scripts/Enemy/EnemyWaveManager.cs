using NaughtyAttributes;
using System.Collections;
using TMPro;
using UnityEngine;

[System.Serializable]
public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField] private GameManager manager;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private EnemyWave[] enemyWaves;

    [SerializeField] private TMP_Text waveDelayText;

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

        UpdateText(0f);
    }

    [Button]
    public void StartRun()
    {
        StartCoroutine(Run());
    }
    public IEnumerator Run()
    {
        foreach (EnemyWave wave in enemyWaves)
        {
            yield return StartCoroutine(StartEnemySpawners());
            _skipWaveDelay = false;

            float timer = waveDelay;
            UpdateText(timer);

            while (timer > 0 && !_skipWaveDelay)
            {
                timer -= Time.deltaTime;
                yield return null;
                UpdateText(Mathf.Ceil(timer));
            }

            UpdateText(0);
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

    private void UpdateText(float time)
    {
        waveDelayText.text = $"{time} second till the next wave starts";
    }
}
