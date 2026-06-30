using FMODUnity;
using NaughtyAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CorpseUpgrade : MonoBehaviour
{
    private enum UpgradeType { AntGain, TowerDMG, TowerUnlock, NestMaxHP}

    [SerializeField] private Button buyButton;

    [Space(30)] 

    [SerializeField] private Upgrade upgrade = new();

    [SerializeField] private UpgradeType upgradeType = UpgradeType.AntGain;
    [SerializeField] private bool _isTowerUpgrade;
    [SerializeField, ShowIf("_isTowerUpgrade")] private int towerIndex;

    [SerializeField] private TMP_Text corpseText;

    private UpgradeDataSaver upgradeDataSaver = new();
    private List<UpgradeClass> upgradeData;


    private GameDataSaver dataSaver = new();
    private GameData _gameData;

    private void Start()
    {
        _gameData = dataSaver.LoadGameData();

        upgradeData = upgradeDataSaver.GetUpgrades();

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

            case UpgradeType.NestMaxHP:
                upgrade.upgradeEvent += NestMaxHPUpgrade;
                break;
        }

        SetCorpseText();
    }
    
    public void TriggerUpgrade()
    {
        upgrade.Trigger();
        SetCorpseText();
    }

    public void AntGainUpgrade()
    {
        if (_gameData.CorpseCount >= upgrade.cost)
        {
            RuntimeManager.PlayOneShot("event:/UI/PurchaseSound");

            upgradeDataSaver.ChangeUpgrade(0, 1);
            _gameData.CorpseCount -= upgrade.cost;
            dataSaver.SaveGameData(_gameData.CorpseCount, _gameData.StageReached, _gameData.WantedStartStage);

            SetCorpseText();

            buyButton.onClick.RemoveListener(TriggerUpgrade);
        }
    }

    public void TowerDMGUpgrade()
    {
        if (_gameData.CorpseCount >= upgrade.cost)
        {
            RuntimeManager.PlayOneShot("event:/UI/PurchaseSound");
            //tower dmg on 2 4 6 8 10
            var towerNum = 2 * towerIndex + 2;
            /*weapon/tower.dmg += amount */
            Debug.Log("tower dmg += amount");
            upgradeDataSaver.ChangeUpgrade(towerNum, 1);
            _gameData.CorpseCount -= upgrade.cost;
            dataSaver.SaveGameData(_gameData.CorpseCount, _gameData.StageReached, _gameData.WantedStartStage);

            SetCorpseText();

            buyButton.onClick.RemoveListener(TriggerUpgrade);
        }
    }

    public void TowerUnlock()
    {
        if (_gameData.CorpseCount >= upgrade.cost)
        {
            RuntimeManager.PlayOneShot("event:/UI/PurchaseSound");
            //tower unlock on 1 3 5 7 9
            var towerNum = 2 * towerIndex + 1;
            /* unlock weapon/tower (set bool to true) */
            Debug.Log("unlocked the ... tower");
            upgradeDataSaver.ChangeUpgrade(towerNum, 1);
            _gameData.CorpseCount -= upgrade.cost;
            dataSaver.SaveGameData(_gameData.CorpseCount, _gameData.StageReached, _gameData.WantedStartStage);

            SetCorpseText();

            buyButton.onClick.RemoveListener(TriggerUpgrade);
        }
    }

    public void NestMaxHPUpgrade()
    {
        if (_gameData.CorpseCount >= upgrade.cost)
        {
            RuntimeManager.PlayOneShot("event:/UI/PurchaseSound");

            upgradeDataSaver.ChangeUpgrade(1000, 1);
            _gameData.CorpseCount -= upgrade.cost;
            dataSaver.SaveGameData(_gameData.CorpseCount, _gameData.StageReached, _gameData.WantedStartStage);

            SetCorpseText();

            buyButton.onClick.RemoveListener(TriggerUpgrade);
        }
    }

    public void SetUIElements()
    {
        upgrade.SetUIElements();
        buyButton.onClick.AddListener(TriggerUpgrade);
    }
    public void SetCorpseText()
    {
        _gameData = dataSaver.LoadGameData();
        corpseText.text = $"{_gameData.CorpseCount}";
    }
}
