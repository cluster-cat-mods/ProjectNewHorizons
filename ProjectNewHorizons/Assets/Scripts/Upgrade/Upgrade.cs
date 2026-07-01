using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Upgrade
{
    [SerializeField] private TMP_Text upgradeNameText;
    [SerializeField] private TMP_Text upgradeDescriptionText;
    [SerializeField] private TMP_Text upgradeCostText;
    [SerializeField] private Image upgradeImage;
    [SerializeField] private Image upgradeSelectButtonImage;

    [Space(30)]

    public string upgradeName = "name of upgrade";
    public string upgradeDescription = "text of the function of the upgrade";
    public int cost = 0;
    public Sprite upgradeSprite;
    public Sprite upgradeButtonSprite;
    public event Action upgradeEvent;

    public void Trigger() => upgradeEvent?.Invoke();

    public void SetUIElements()
    {
        upgradeNameText.text = upgradeName;
        upgradeDescriptionText.text = upgradeDescription;
        upgradeCostText.text = cost + "";
        upgradeImage.sprite = upgradeSprite;
    }

    public void SetButtonImage()
    {
        upgradeSelectButtonImage.sprite = upgradeButtonSprite;
    }

    public void ChangeText(string newName, string newDescription)
    {
        upgradeName = newName;
        upgradeDescription = newDescription;

        SetUIElements();
    }
    public void ChangeImage(Sprite newSprite)
    {
        upgradeButtonSprite = newSprite;
        SetUIElements();
        SetButtonImage();
    }
}
