using System.IO;
using Unity.Collections;
using UnityEngine;

public class GameDataSaver
{
    private string path;
    private string json;
    
    public void SaveGameData(int  corpseCount, StageReached[] maxStageReached, int wantedStartStage)
    {
        GameData gameData = new GameData(corpseCount, maxStageReached, wantedStartStage);
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
        File.WriteAllText(path, json);
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
    public int CorpseCount;
    public StageReached[] StageReached;
    public int WantedStartStage;
    
    public GameData(int corpseCount, StageReached[] maxStageReached, int wantedStartStage)
    {
        CorpseCount = corpseCount;
        StageReached = maxStageReached;
        WantedStartStage = wantedStartStage;
    }
}
