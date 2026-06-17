using NaughtyAttributes;
using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    [SerializeField] private int startingHiveMaxHP;
    [SerializeField] private bool alive = true;

    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text antText;
    [SerializeField] private TMP_Text HiveHPText;

    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private EnemyWave[] enemyWaves;
    public int hiveMaxHP { get; private set; }
    public int hiveHP { get; private set; }
    public int2 antCount { get; private set; }
    public int antGain { get; private set; }
    public int coins { get; private set; }
    public int wave { get; private set; }
    public int stage { get; private set; }

    private void Start()
    {
        if (enemySpawner == null)
        {
            enemySpawner = FindAnyObjectByType<EnemySpawner>();
        }

        antGain = 1;
        SetStartStats();
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
    }

    private IEnumerator AliveChecker()
    {
        while (alive)
        {
            if (hiveHP <= 0)
            {
                alive = false;
            }
            yield return null;
        }

        Debug.Log("you died");
        yield return new WaitForSeconds(1);
        SetStartStats();
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
        coins = coins + amount;
        //Debug.Log($"you have {coins} coins");
        SetCoinText();
    }
    public void LoseCoins(int amount)
    {
        coins = coins - amount;
        //Debug.Log($"you have {coins} coins");
        SetCoinText();
    }
    public void LoseHP(int amount)
    {
        hiveHP = hiveHP - amount;
        //Debug.Log($"you have {hiveHP} hp");
        SetHiveHPText();
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
        coinText.text = $"{coins} coin(s)";
    }

    public void SetAntText()
    {
        antText.text = $"you have {antCount.x}/{antCount.y} ant(s)";
    }
    public void SetHiveHPText()
    {
        HiveHPText.text = $"{hiveHP}/{hiveMaxHP} HP";
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

    [Button]
    public void StartEnemySpawners()
    {
        StartCoroutine(enemySpawner.StartWave(enemyWaves, wave));
    }
}
