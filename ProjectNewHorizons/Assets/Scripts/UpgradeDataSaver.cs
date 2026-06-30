using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UpgradeDataSaver
{
    private string path;
    private string json;

    public void ChangeUpgrade(int ID, int count)
    {
#if (UNITY_WEBGL && !UNITY_EDITOR)
        path = System.IO.Path.Combine("idbfs", Application.productName);
        Debug.Log($"{path}");
        if (!File.Exists(path)) 
        { 
            Directory.CreateDirectory(path);
        }
        path = System.IO.Path.Combine(path, "saveAntUpgradeData");
        Debug.Log($"{path}");
#else
        path = Application.persistentDataPath + "/upgradeData.json";
#endif
        if (!File.Exists(path))
        {
            var emptyData = new UpgradeData();
            File.WriteAllText(path, JsonUtility.ToJson(emptyData, true));
        }

        var fileText = File.ReadAllText(path);
        UpgradeData data = JsonUtility.FromJson<UpgradeData>(fileText);

        if (data == null)
        {
            data = new UpgradeData();
        }

        List<UpgradeClass> upgrades = data.upgrades;
        bool foundUpgrade = false;
        for (int i = 0; i < upgrades.Count; i++)
        {
            if (upgrades[i].ID != ID) continue;
            upgrades[i].AddCount(count);
            if (upgrades[i].count <= 0)
            {
                upgrades.RemoveAt(i);
                i--;
            }
            foundUpgrade = true;
            break;
        }
        if (!foundUpgrade)  upgrades.Add(new   UpgradeClass(ID, count));

        data.upgrades = upgrades;

        json = JsonUtility.ToJson(data, true);

        File.WriteAllText(path, json);
    }

    public List<UpgradeClass> GetUpgrades()
    {
#if (UNITY_WEBGL && !UNITY_EDITOR)
        path = System.IO.Path.Combine("idbfs", Application.productName);
        path = System.IO.Path.Combine(path, "saveAntUpgradeData");
        if (!File.Exists(path)) 
        { 
            Directory.CreateDirectory(path);
        }
        Debug.Log($"{path}");
#else
        path = Application.persistentDataPath + "/upgradeData.json";
#endif
        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
            var rawJsonOutput = JsonUtility.FromJson<UpgradeData>(json);
            return rawJsonOutput.upgrades;
        }
        else
        {
            var emptyData = new UpgradeData();
            File.WriteAllText(path, JsonUtility.ToJson(emptyData, true));
            return null;
        }
    }   
}

[System.Serializable]
public class UpgradeData
{
    public List<UpgradeClass> upgrades = new();
}

[System.Serializable]
public class UpgradeClass
{
    public int ID;
    public int count;

    public void AddCount(int countIncrease)
    {
        count += countIncrease;
    }
    
    public UpgradeClass(int ID, int count)
    {
        this.ID = ID;
        this.count = count;
    }
}
