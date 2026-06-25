using System.IO;
using Unity.Collections;
using UnityEngine;

public class GameDataSaver : MonoBehaviour
{
    private string path = Application.persistentDataPath + "/gameData.json";
    
    public void SaveGameData(int  corpseCount, int waveNum, int stageNum)
    {
        GameData gameData = new GameData(corpseCount, waveNum, stageNum);
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
    public int WaveNum {get; private set;}
    public int StageNum {get; private set;}
    
    public GameData(int corpseCount, int waveNum, int stageNum)
    {
        this.CorpseCount = corpseCount;
        this.WaveNum = waveNum;
        this.StageNum = stageNum;
    }
}
