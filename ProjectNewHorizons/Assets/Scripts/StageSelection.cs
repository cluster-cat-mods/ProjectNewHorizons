using UnityEngine;

public class StageSelection : MonoBehaviour
{
    private GameDataSaver dataSaver = new();

    private int _corpseCount;
    private StageReached[] _reachedStage; 

    void Start()
    {
        var data = dataSaver.LoadGameData();
        if (data != null)
        {
            _reachedStage = data.StageReached;
            _corpseCount = data.CorpseCount;
        }
    }

    public void StartAtStageNum(int StageIndex)
    {
        if (_reachedStage[StageIndex].amountOfTimesReached > 3)
        {
            dataSaver.SaveGameData(_corpseCount, _reachedStage, StageIndex);
            var stageText = $"set the stage to {StageIndex}";
        }
        else
        {
            var stageText = $"stage {StageIndex} was not reached enough times yet";
        }
    }
}
