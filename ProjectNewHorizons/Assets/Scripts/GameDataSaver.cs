using System.IO;
using Unity.Collections;
using UnityEngine;

public class GameDataSaver
{
    private string path;
    private string json;
    
    public void SaveGameData(int  corpseCount, StageReached[] maxStageReached)
    {
        GameData gameData = new GameData(corpseCount, maxStageReached);
        json = JsonUtility.ToJson(gameData);

#if (UNITY_WEBGL && !UNITY_EDITOR)
        path = System.IO.Path.Combine("idbfs", Application.productName);
        if (!File.Exists(path)) 
        { 
            Directory.CreateDirectory(path);
        }
        path = System.IO.Path.Combine(path, "saveAntData");
#else
        path = Application.persistentDataPath + "/gameData.json";
#endif
        File.WriteAllText(path, JsonUtility.ToJson(JsonUtility.ToJson(gameData)));
    }

    public GameData LoadGameData()
    {
#if (UNITY_WEBGL && !UNITY_EDITOR)
        path = System.IO.Path.Combine("idbfs", Application.productName);
        path = System.IO.Path.Combine(path, "saveAntData");
#else
        path = Application.persistentDataPath + "/gameData.json";
#endif
        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
            return JsonUtility.FromJson<GameData>(json);
        }
        else
        {
            return null;
        }
    }
}

[System.Serializable]
public class GameData
{
    public int CorpseCount {get; private set;}
    public StageReached[] StageReached {get; private set;}
    
    public GameData(int corpseCount, StageReached[] maxStageReached)
    {
        this.CorpseCount = corpseCount;
        this.StageReached = maxStageReached;
    }
}
