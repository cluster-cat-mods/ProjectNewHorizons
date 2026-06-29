using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CorpseUpgrade : MonoBehaviour
{
    private enum UpgradeType { AntGain, TowerDMG, TowerUnlock}

    [SerializeField] private GameManager manager;
    [SerializeField] private TowerManager towerManager;

    [SerializeField] private Upgrade upgrade = new();

    [SerializeField] private UpgradeType upgradeType = UpgradeType.AntGain;

    private UpgradeDataSaver upgradeDataSaver = new();
    private List<UpgradeClass> upgradeData;

    [SerializeField] private TMP_Text corpseText;

    private GameDataSaver dataSaver = new();
    private GameData _gameData;

    private void Start()
    {
        _gameData = dataSaver.LoadGameData();

        if (manager == null)
        {
            manager = FindAnyObjectByType<GameManager>();
        }

        if (towerManager == null)
        {
            towerManager = FindAnyObjectByType<TowerManager>();
        }

        upgradeData = upgradeDataSaver.GetUpgrades();

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
            _gameData.CorpseCount -= upgrade.cost;
            dataSaver.SaveGameData(_gameData.CorpseCount, _gameData.StageReached, _gameData.WantedStartStage);
            SetCorpseText();
        }
    }

    public void TowerDMGUpgrade()
    {
        if (manager.corpse >= upgrade.cost)
        {
            /*weapon/tower.dmg += amount */
            Debug.Log("tower dmg += amount");
            _gameData.CorpseCount -= upgrade.cost;
            dataSaver.SaveGameData(_gameData.CorpseCount, _gameData.StageReached, _gameData.WantedStartStage);
            SetCorpseText();
        }
    }

    public void TowerUnlock()
    {
        if (manager.corpse >= upgrade.cost)
        {
            /* unlock weapon/tower (set bool to true) */
            Debug.Log("unlocked the ... tower");
            //manager

            _gameData.CorpseCount -= upgrade.cost;
            dataSaver.SaveGameData(_gameData.CorpseCount, _gameData.StageReached, _gameData.WantedStartStage);
            SetCorpseText();
        }
    }
    public void SetCorpseText()
    {
        corpseText.text = $"{_gameData.CorpseCount}";
    }
}
