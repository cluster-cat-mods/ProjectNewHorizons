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
    public int hiveMaxHP { get; private set; }
    public int hiveHP { get; private set; }
    public int2 antCount { get; private set; }
    public int antGain { get; private set; }
    public int coins { get; private set; }

    private void Start()
    {
        hiveMaxHP = startingHiveMaxHP;
        hiveHP = hiveMaxHP;
        antGain = 1;
        StartCoroutine(AntGainOvertime());
    }

    private void Update()
    {
        if (alive) 
        {
            
        }
        else
        {

        }
    }
    private IEnumerator AntGainOvertime()
    {
        while (alive)
        {
            yield return new WaitForSeconds(1);
            antCount = new(antCount.x, antCount.y + antGain);
            Debug.Log($"you have {antCount.x}/{antCount.y} ants");
            SetAntText();
        }
    }
    public void GainCoins(int amount)
    {
        coins = coins + amount;
        Debug.Log($"you have {coins} coins");
        SetCoinText();
    }
    public void LoseCoins(int amount)
    {
        coins = coins - amount;
        Debug.Log($"you have {coins} coins");
        SetCoinText();
    }
    public void LoseHP(int amount)
    {
        hiveHP = hiveHP - amount;
        Debug.Log($"you have {hiveHP} hp");
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
}
