using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AntpediaUISetter : MonoBehaviour
{

    [SerializeField] private EnemyStats enemyStats;

    [SerializeField] private TMP_Text antNameText;
    [SerializeField] private TMP_Text antDescriptionText;
    [SerializeField] private Image antImage;

    [Space(30)]

    public string antName = "name of ant";
    public string antDescription = "description of ant";
    public Sprite antSprite;

    public void SetUIElements()
    {
        antNameText.text = antName;
        var descriptionString = $"{antDescription} \n " +
                                $"HP: {enemyStats.BaseStats.hp} \n" +
                                $"Damage: {enemyStats.BaseStats.damage} \n" +
                                $"Speed: {enemyStats.BaseStats.speed} \n" +
                                $"CorpseBounty: {enemyStats.BaseStats.corpseBounty} \n";
        antDescriptionText.text = descriptionString;
        antImage.sprite = antSprite;
    }

    public void ChangeText(string newName, string newDescription)
    {
        antName = newName;
        antDescription = newDescription;

        SetUIElements();
    }
    public void ChangeImage(Sprite newSprite)
    {
        antSprite = newSprite;
        SetUIElements();
    }
}
