using NaughtyAttributes;
using UnityEngine;

public class DevFunctions : MonoBehaviour
{
    private GameDataSaver _dataSaver = new();
    private UpgradeDataSaver _upgradeDataSaver = new();

    [Button]
    public void ResetFullSaveFile()
    {
        _dataSaver.ResetData();
        _upgradeDataSaver.ResetUpgrades();
    }
    [Button]
    public void ResetGameData()
    {
        _dataSaver.ResetData();
    }

    [Button]
    public void ResetUpgrades()
    {
        _upgradeDataSaver.ResetUpgrades();
    }
}
