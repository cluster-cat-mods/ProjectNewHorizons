using NaughtyAttributes;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    [SerializeField] private int startingHiveMaxHP;
    [SerializeField] private bool alive;
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
        if (!alive) StopCoroutine(AntGainOvertime());
        int2 newMaxAntCount = antCount;
        yield return new WaitForSeconds(1);
        newMaxAntCount.y += antGain;
        antCount = newMaxAntCount;
        Debug.Log($"you have {antCount} ants");
        StartCoroutine(AntGainOvertime());
    }
    public void GainCoins(int amount)
    {
        int newCoins = coins;
        newCoins += amount;
        coins = newCoins;
        Debug.Log($"you have {coins} coins");
    }
    public void LoseCoins(int amount)
    {
        int newCoins = coins;
        newCoins -= amount;
        coins = newCoins;
        Debug.Log($"you have {coins} coins");
    }
    public void LoseHP(int amount)
    {
        int newHiveHP = hiveHP;
        newHiveHP -= amount;
        hiveHP = newHiveHP;
        Debug.Log($"you have {hiveHP} hp");
    }
    public void IncreaseAntGain(int amount)
    {
        int newAntGain = antGain;
        newAntGain += amount;
        antGain = newAntGain;
    }

    public void AllocateAnt(int amount)
    {
        int2 newAllocatedAntCount = antCount;
        newAllocatedAntCount.x += amount;
        antCount = newAllocatedAntCount;
    }
}
