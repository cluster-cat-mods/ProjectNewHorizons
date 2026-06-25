using NaughtyAttributes;
using NUnit.Framework;
using System.Collections;
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

    [SerializeField] private UnityEvent dieEvent;
    [SerializeField] private UnityEvent regainLifeEvent;
    public int hiveMaxHP { get; private set; }
    public int hiveHP { get; private set; }
    public int2 antCount { get; private set; }
    public int antGain { get; private set; }
    public int corpse { get; private set; }
    public int wave { get; private set; }
    public int stage { get; private set; }

    private void Start()
    {
        if (enemyWaveManager == null)
        {
            enemyWaveManager = FindAnyObjectByType<EnemyWaveManager>();
        }

        antGain = 1;
    }

    private void SetStartStats()
    {
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
    }

    private IEnumerator DieEffect()
    {
        while (volume[0].weight < 1)
        {
            yield return null;
            volume[0].weight += .004f;
        }
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
            yield return new WaitForSeconds(1);
            antCount = new(antCount.x, antCount.y + antGain);
            //Debug.Log($"you have {antCount.x}/{antCount.y} ants");
            SetAntText();
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
        while (volume[1].weight > 0)
        {
            yield return null;
            volume[1].weight -= .01f;
        }
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
        }
    }

    private IEnumerator NextWaveEffect()
    {
        while (volume[2].weight < 1)
        {
            yield return null;
            volume[2].weight += .2f;
        }
        yield return new WaitForSeconds(.5f);
        while (volume[2].weight > 0)
        {
            yield return null;
            volume[2].weight -= .2f;
        }
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
