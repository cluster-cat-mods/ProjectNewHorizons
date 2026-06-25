using System;
using UnityEngine;

public class CoinUpgrade : MonoBehaviour
{
    private enum UpgradeType { AntGain, TowerDMG, TowerUnlock}

    [SerializeField] private GameManager manager;

    [SerializeField] private Upgrade upgrade = new();

    [SerializeField] private UpgradeType upgradeType = UpgradeType.AntGain;

    private void Start()
    {
        if (manager == null)
        {
            manager = FindAnyObjectByType<GameManager>();
        }

        upgrade.SetTexts();

        switch (upgradeType)
        {
            case UpgradeType.AntGain:
                upgrade.upgradeEvent += AntGainUpgrade;
                break;

            case UpgradeType.TowerDMG:
                upgrade.upgradeEvent += TowerDMGUpgrade;
                break;

            case UpgradeType.TowerUnlock:
                upgrade.upgradeEvent += TowerUnlock;
                break;

        }
    }
    
    public void TriggerUpgrade()
    {
        upgrade.Trigger();
    }

    public void AntGainUpgrade()
    {
        if (manager.corpse >= upgrade.cost)
        {
            manager.IncreaseAntGain(1);
            manager.LoseCoins(upgrade.cost);
        }
    }

    public void TowerDMGUpgrade()
    {
        if (manager.corpse >= upgrade.cost)
        {
            /*weapon/tower.dmg += amount */
            Debug.Log("tower dmg += amount");
            manager.LoseCoins(upgrade.cost);
        }
    }

    public void TowerUnlock()
    {
        if (manager.corpse >= upgrade.cost)
        {
            /* unlock weapon/tower (set bool to true) */
            Debug.Log("unlocked the ... tower");
            manager.LoseCoins(upgrade.cost);
        }
    }
}
