using System.IO;
using Unity.Collections;
using UnityEngine;

public class GameDataSaver : MonoBehaviour
{
    private string path = Application.persistentDataPath + "/gameData.json";
    
    public void SaveGameData(int  corpseCount, StageReached[] maxStageReached)
    {
        GameData gameData = new GameData(corpseCount, maxStageReached);
        File.WriteAllText(path, JsonUtility.ToJson(JsonUtility.ToJson(gameData)));
    }

    public GameData LoadGameData()
    {
        return JsonUtility.FromJson<GameData>(File.ReadAllText(path));
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
