using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UpgradeDataSaver
{
    private string path =  Application.persistentDataPath + "/upgrades.json";

    public void ChangeUpgrade(int ID, int count)
    {
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
        
        File.WriteAllText(path, JsonUtility.ToJson(upgrades, true));
    }

    public List<UpgradeClass> GetUpgrades()
    {
        return JsonUtility.FromJson<List<UpgradeClass>>(File.ReadAllText(path));
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
