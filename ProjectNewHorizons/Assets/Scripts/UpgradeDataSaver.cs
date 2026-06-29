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
        if (!File.Exists(path)) 
        { 
            Directory.CreateDirectory(path);
        }
        path = System.IO.Path.Combine(path, "saveAntUpgradeData");
#else
        path = Application.persistentDataPath + "/upgradeData.json";
#endif
        List<UpgradeClass> upgrades = JsonUtility.FromJson<List<UpgradeClass>>(File.ReadAllText(path));
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

        json = JsonUtility.ToJson(upgrades, true);

        File.WriteAllText(path, json);
    }

    public List<UpgradeClass> GetUpgrades()
    {
#if (UNITY_WEBGL && !UNITY_EDITOR)
        path = System.IO.Path.Combine("idbfs", Application.productName);
        path = System.IO.Path.Combine(path, "saveAntUpgradeData");
#else
        path = Application.persistentDataPath + "/upgradeData.json";
#endif
        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
            //return JsonUtility.FromJson<GameData>(json);
            return JsonUtility.FromJson<List<UpgradeClass>>(File.ReadAllText(path));
        }
        else
        {
            return null;
        }
    }   
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
