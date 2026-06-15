using System;
using UnityEngine;

public class CoinUpgrade : MonoBehaviour
{
    [SerializeField] private GameManager manager;

    [SerializeField] private Upgrade u1 = new();

    private void Start()
    {
        if (manager == null)
        {
            manager = FindAnyObjectByType<GameManager>();
        }

        u1.SetTexts();
        u1.upgradeEvent += Upgrade1;
        u1.Trigger();
    }
    

    public void Upgrade1()
    {
        if (manager.coins >= u1.cost)
        {
            manager.IncreaseAntGain(1);
            manager.LoseCoins(u1.cost);
        }
    }
}
