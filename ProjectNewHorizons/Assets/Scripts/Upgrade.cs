using System;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Upgrade
{
    [SerializeField] private TMP_Text upgradeNameText;
    [SerializeField] private TMP_Text upgradeDescriptionText;

    public string upgradeName = "name of upgrade";
    public int cost = 0;
    public string upgradeDescription = "text of the function of the upgrade";
    public event Action upgradeEvent;

    public void Trigger() => upgradeEvent?.Invoke();

    public void SetTexts()
    {
        upgradeNameText.text = upgradeName;
        upgradeDescriptionText.text = upgradeDescription;
    }
}
