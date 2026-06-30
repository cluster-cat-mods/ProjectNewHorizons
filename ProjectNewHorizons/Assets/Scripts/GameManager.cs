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
    [SerializeField] private int startingNestMaxHP;

    [SerializeField] private TMP_Text corpseText;
    [SerializeField] private TMP_Text antText;
    [SerializeField] private TMP_Text nestHPText;

    [SerializeField] private EnemyWaveManager enemyWaveManager;
    //[SerializeField] private TowerManager towerManager;

    [SerializeField] private Volume[] volume;
    
    public GameObject nest;

    public bool alive = true;

    [SerializeField] private UnityEvent startEvent;
    [SerializeField] private UnityEvent dieEvent;
    [SerializeField] private UnityEvent regainLifeEvent;

    [SerializeField, AllowNesting] private List<StageReached> reachedStageList = new();
    public int nestMaxHP { get; private set; }
    public int nestHP { get; private set; }
    public int2 antCount { get; private set; }
    public int antGain { get; private set; }
    public int corpse { get; private set; }
    public int wave { get; private set; }
    public int stage { get; private set; }

    private GameDataSaver dataSaver = new();
    private UpgradeDataSaver upgradeDataSaver = new();

    private void Start()
    {
        if (enemyWaveManager == null)
        {
            enemyWaveManager = FindAnyObjectByType<EnemyWaveManager>();
        }

        //if (towerManager == null)
        //{
        //    towerManager = FindAnyObjectByType<TowerManager>();
        //}

        startEvent?.Invoke();

        //for (int i = 0; i < 3; i++)
        //{
        //    StageReached reachedStage = new StageReached { stageNumber = i, amountOfTimesReached = 0 };
        //    reachedStageList.Add(reachedStage);
        //}
        //towerManager.SetUpgradeValues();
    }

    private void SetStartStats()
    {
        var gameData = dataSaver.LoadGameData();
        if (gameData != null)
        {
            reachedStageList.Clear();

            corpse = gameData.CorpseCount;

            foreach (var reachedStage in gameData.StageReached)
            {
                reachedStageList.Add(reachedStage);
            }

            SetWantedStage(gameData.WantedStartStage);
        }

        antGain = 1;
        nestMaxHP = startingNestMaxHP;

        var upgradeDataList = upgradeDataSaver.GetUpgrades();
        if (upgradeDataList != null)
        {
            foreach (var upgrade in upgradeDataList)
            {
                if (upgrade.ID == 0)
                {
                    IncreaseAntGain(upgrade.count);
                }

                if (upgrade.ID == 1000)
                {
                    IncreaseMaxHP(upgrade.count);
                }
            }
        }

        nestHP = nestMaxHP;
        antCount = 0;
        alive = true;
        SetAntText();
        SetNestHPText();
        SetCorpseText();
        StartCoroutine(AliveChecker());
        StartCoroutine(AntGainOvertime());
        volume[0].weight = 0;
    }

    private IEnumerator AliveChecker()
    {
        while (alive)
        {
            if (nestHP <= 0)
            {
                alive = false;
                //Destroy(nest);
            }

            yield return null;
        }
        Debug.Log("you died");
        nest.SetActive(false);
        dieEvent?.Invoke();
        yield return new WaitForSeconds(.1f);
        StartCoroutine(DieEffect());
        DestroyEnemies();
        enemyWaveManager.StopAllCoroutines();

        SaveData();
        //dataSaver.SaveGameData(corpse, saveReachedStages.ToArray(), 0);
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
        nest.SetActive(true);
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
    public void GainCorpse(int amount)
    {
        corpse = corpse + amount;
        //Debug.Log($"you have {corpse} corpse");
        SetCorpseText();
    }
    public void LoseCorpse(int amount)
    {
        corpse = corpse - amount;
        //Debug.Log($"you have {corpse} corpse");
        SetCorpseText();
    }
    public void LoseHP(int amount)
    {
        volume[1].weight = 1;
        nestHP = nestHP - amount;
        //Debug.Log($"you have {nestHP} hp");
        SetNestHPText();

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

    public void IncreaseMaxHP(int amount)
    {
        nestMaxHP = nestMaxHP + amount;
        SetNestHPText();
    }

    public void AllocateAnt(int amount)
    {
        if (antCount.x + amount <= antCount.y)
        {
            antCount = new(antCount.x + amount, antCount.y);
            SetAntText();
        }
    }

    public void SetCorpseText()
    {
        corpseText.text = $"{corpse}";
    }

    public void SetAntText()
    {
        antText.text = $"{antCount.x}/{antCount.y}";
    }
    public void SetNestHPText()
    {
        nestHPText.text = $"{nestHP}/{nestMaxHP}";
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

    public void SaveData()
    {
        List<StageReached> saveReachedStages = new();

        foreach (var reachedStage in reachedStageList)
        {
            saveReachedStages.Add(reachedStage);
        }

        dataSaver.SaveGameData(corpse, saveReachedStages.ToArray(), 0);
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
    public void Get10Corpse()
    {
        GainCorpse(10);
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
