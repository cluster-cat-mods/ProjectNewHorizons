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
    [SerializeField] private bool isTowerUpgrade;
    [SerializeField, ShowIf("isTowerUpgrade")] private int towerIndex;
    [SerializeField, ShowIf("isTowerUpgrade")] private bool needsToChange;
    [SerializeField, ShowIf("needsToChange")] private Sprite upgradeButtonSpriteDMGUp;
    [SerializeField, ShowIf("needsToChange")] private string upgradeNameDMGUp;
    [SerializeField, ShowIf("needsToChange")] private string upgradeDescriptionDMGUp;

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

        foreach (var upgrade in upgradeData)
        {
            if (upgrade.ID == 2 * towerIndex + 1)
            {
                ChangeToTowerDMGUpgrade();
            }
        }
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

            ChangeToTowerDMGUpgrade();
        }
    }


    void ChangeToTowerDMGUpgrade()
    {
        upgradeType = UpgradeType.TowerDMG;
        upgrade.ChangeImage(upgradeButtonSpriteDMGUp);
        upgrade.ChangeText(upgradeNameDMGUp, upgradeDescriptionDMGUp);

        resetEvent();
    }

    void resetEvent()
    {
        upgrade.upgradeEvent -= TowerUnlock;
        upgrade.upgradeEvent += TowerDMGUpgrade;
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
        }
    }

    public void SetUIElements()
    {
        upgrade.SetUIElements();
        buyButton.onClick.AddListener(TriggerUpgrade);
    }

    public void SetButtonImage()
    {
        upgrade.SetButtonImage();
    }
    public void SetCorpseText()
    {
        _gameData = dataSaver.LoadGameData();
        corpseText.text = $"{_gameData.CorpseCount}";
    }
    public void RemoveListener()
    {
        //if (buyButton.onClick.)
        buyButton.onClick.RemoveListener(TriggerUpgrade);
    }
}
