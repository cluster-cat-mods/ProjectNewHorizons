using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

[System.Serializable]
public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField] private GameManager manager;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private EnemyWave[] enemyWaves;
    private List<EnemyWave> savedEnemyWaves = new();

    [SerializeField] private TMP_Text waveDelayText;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text stageText;

    [SerializeField] private float waveDelay;

    [SerializeField] private GameObject skipWaveDelayButton;

    private bool _skipWaveDelay = false;

    [HideInInspector] public float timer = 0;

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

        //Debug.Log($"Manager: {manager}");
        //Debug.Log($"Spawner: {enemySpawner}");

        for (int i = 0; i < enemyWaves.Length; i++)
        {
            // Creates a unique runtime-only duplicate of the Scriptable Object
            savedEnemyWaves.Add(enemyWaves[i]);
        }

        UpdateText();

        skipWaveDelayButton.SetActive(false);
    }

    private void Update()
    {
        UpdateText();
    }

    public void setEnemyWaves()
    {
        // Do this when initializing your enemyWaves array/list at Start()
        for (int i = 0; i < enemyWaves.Length; i++)
        {
            // Creates a unique runtime-only duplicate of the Scriptable Object
            enemyWaves[i] = Instantiate(savedEnemyWaves[i]);
        }
    }
    //[Button]
    public void StartRun()
    {
        StartCoroutine(Run());
    }
    public IEnumerator Run()
    {
        //manager.SetWave(0);
        setEnemyWaves();
        StartCoroutine(enemySpawner.PathIndicator());

        for (int i = manager.wave; i < enemyWaves.Length; i++)
        {
            if (!manager.alive) break;

            var wave = enemyWaves[i];

            yield return StartCoroutine(StartEnemySpawners());
            _skipWaveDelay = false;

            timer = waveDelay;
            while (timer > 0 && !_skipWaveDelay)
            {
                skipWaveDelayButton.SetActive(true);
                timer -= Time.deltaTime;

                yield return null;
            }

            skipWaveDelayButton.SetActive(false);
            timer = 0;
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

    private void UpdateText()
    {
        if (waveDelayText == null)
        {
            Debug.LogError("waveDelayText is null!");
        }
        else
        {
            if (timer < 0)
            {
                timer = 0;
            }
            var time = Mathf.Ceil(timer);
            waveDelayText.text = $"{time} second(s) till next wave";
        }

        if (waveText == null) 
        {
            Debug.LogError("waveDelayText is null!");
        }
        else
        {
            waveText.text = $"Wave : {manager.wave + 1}";
        }

        if (stageText == null)
        {
            Debug.LogError("waveDelayText is null!");
        }
        else
        {
            stageText.text = $"Stage : {manager.stage + 1}";
        }
    }

    public void Replay()
    {
        UpdateText();

        skipWaveDelayButton.SetActive(false);
    }
}
