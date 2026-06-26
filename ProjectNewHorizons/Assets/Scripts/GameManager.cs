using NaughtyAttributes;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour 
{
    [SerializeField] private int startingHiveMaxHP;

    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text antText;
    [SerializeField] private TMP_Text HiveHPText;

    [SerializeField] private EnemyWaveManager enemyWaveManager;

    [SerializeField] private Volume[] volume;
    
    public GameObject hive;

    public bool alive = true;

    [SerializeField] private UnityEvent startEvent;
    [SerializeField] private UnityEvent dieEvent;
    [SerializeField] private UnityEvent regainLifeEvent;

    [SerializeField, AllowNesting] private List<StageReached> reachedStageList = new();
    public int hiveMaxHP { get; private set; }
    public int hiveHP { get; private set; }
    public int2 antCount { get; private set; }
    public int antGain { get; private set; }
    public int corpse { get; private set; }
    public int wave { get; private set; }
    public int stage { get; private set; }

    private GameDataSaver dataSaver = new();

    private void Start()
    {
        if (enemyWaveManager == null)
        {
            enemyWaveManager = FindAnyObjectByType<EnemyWaveManager>();
        }

        antGain = 1;

        startEvent?.Invoke();

        //for (int i = 0; i < 3; i++)
        //{
        //    StageReached reachedStage = new StageReached { stageNumber = i, amountOfTimesReached = 0 };
        //    reachedStageList.Add(reachedStage);
        //}

    }

    private void SetStartStats()
    {
        var data = dataSaver.LoadGameData();
        if (data != null)
        {
            reachedStageList.Clear();

            corpse = data.CorpseCount;

            foreach (var reachedStage in data.StageReached)
            {
                reachedStageList.Add(reachedStage);
            }
        }

        hiveMaxHP = startingHiveMaxHP;
        hiveHP = hiveMaxHP;
        antCount = 0;
        alive = true;
        SetAntText();
        SetHiveHPText();
        StartCoroutine(AliveChecker());
        StartCoroutine(AntGainOvertime());
        volume[0].weight = 0;
    }

    private IEnumerator AliveChecker()
    {
        while (alive)
        {
            if (hiveHP <= 0)
            {
                alive = false;
                //Destroy(hive);
            }

            yield return null;
        }
        Debug.Log("you died");
        hive.SetActive(false);
        dieEvent?.Invoke();
        yield return new WaitForSeconds(.1f);
        StartCoroutine(DieEffect());
        DestroyEnemies();
        enemyWaveManager.StopAllCoroutines();

        List<StageReached> saveReachedStages = new();

        foreach (var reachedStage in reachedStageList)
        {
            saveReachedStages.Add(reachedStage);
        }

        dataSaver.SaveGameData(corpse, saveReachedStages.ToArray());
        //Debug.Log($"corpses == {corpse}");
        //Debug.Log($"saved stages == {saveReachedStages}");
        //Debug.Log("sgt")
    }

    private IEnumerator DieEffect()
    {
        yield return EffectGain(0, .004f);
    }
    private void GainLife()
    {
        hive.SetActive(true);
        SetStartStats();
        regainLifeEvent?.Invoke();
    }

    private void DestroyEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

    }
    private IEnumerator AntGainOvertime()
    {
        while (alive)
        {
            if (enemyWaveManager.timer == 0)
            {
                yield return new WaitForSeconds(1);
                antCount = new(antCount.x, antCount.y + antGain);
                //Debug.Log($"you have {antCount.x}/{antCount.y} ants");
                SetAntText();
            }
            else
            {
                yield return null;
            }
        }
    }
    public void GainCoins(int amount)
    {
        corpse = corpse + amount;
        //Debug.Log($"you have {corpse} corpse");
        SetCoinText();
    }
    public void LoseCoins(int amount)
    {
        corpse = corpse - amount;
        //Debug.Log($"you have {corpse} corpse");
        SetCoinText();
    }
    public void LoseHP(int amount)
    {
        volume[1].weight = 1;
        hiveHP = hiveHP - amount;
        //Debug.Log($"you have {hiveHP} hp");
        SetHiveHPText();

        StartCoroutine(LoseHpEffect());
    }
    private IEnumerator LoseHpEffect()
    {
        yield return new WaitForSeconds(.3f);
        yield return EffectDecay(1, .01f);
    }
    public void IncreaseAntGain(int amount)
    {
        antGain = antGain + amount;
        SetAntText();
    }

    public void AllocateAnt(int amount)
    {
        if (antCount.x + amount <= antCount.y)
        {
            antCount = new(antCount.x + amount, antCount.y);
            SetAntText();
        }
    }

    public void SetCoinText()
    {
        coinText.text = $"{corpse}";
    }

    public void SetAntText()
    {
        antText.text = $"{antCount.x}/{antCount.y}";
    }
    public void SetHiveHPText()
    {
        HiveHPText.text = $"{hiveHP}/{hiveMaxHP}";
    }

    public void SetWantedStage(int amount)
    {
        stage = amount;
        SetWave(0);
    }

    public void SetWave(int amount)
    {
        wave = amount + 5 * stage;
    }

    public void IncreaseWave()
    {
        wave++;
        StartCoroutine(NextWaveEffect());

        if (wave % 5 == 0)
        {
            stage++;

            StageReached existingStageReached = reachedStageList.Find(x => x.stageNumber == stage);

            if (existingStageReached != null)
            {
                existingStageReached.amountOfTimesReached++;
            }
            else
            {
                reachedStageList.Add(new StageReached
                {
                    stageNumber = stage,
                    amountOfTimesReached = 1
                });
            }
        }
    }

    private IEnumerator NextWaveEffect()
    {
        yield return EffectGain(2, .1f);
        yield return new WaitForSeconds(1);
        yield return EffectDecay(2, .05f);
    }

    private IEnumerator EffectGain(int index, float gainAmount)
    {
        while (volume[index].weight < 1)
        {
            yield return null;
            volume[index].weight += gainAmount;
        }
    }

    private IEnumerator EffectDecay(int index, float decayAmount)
    {
        while (volume[index].weight > 0)
        {
            yield return null;
            volume[index].weight -= decayAmount;
        }
    }

    //[Button]
    public void StartRun()
    {
        GainLife();
    }
    /* testing functions and vars */
    [Button]
    public void Get10Coins()
    {
        GainCoins(10);
    }

    [Button]
    public void Allocate1Ant()
    {
        AllocateAnt(1);
    }
    [Button]
    public void Lose2Hp()
    {
        LoseHP(2);
    }

}

[Serializable]
public class StageReached
{
    public int stageNumber;
    public int amountOfTimesReached;
}
