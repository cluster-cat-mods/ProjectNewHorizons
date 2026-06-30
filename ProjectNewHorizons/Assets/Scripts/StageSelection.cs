using UnityEngine;

public class StageSelection : MonoBehaviour
{
    private GameDataSaver dataSaver = new();

    private int _corpseCount;
    private StageReached[] _reachedStage; 

    void Start()
    {
        LoadData();
    }

    void LoadData()
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
        LoadData();

        if (_reachedStage.Length <= StageIndex)
        {
            var emptyReachedStageArray = new StageReached[StageIndex + 1];

            for (int i = 0; i < _reachedStage.Length; i++) 
            {
                if (_reachedStage[i] != null)
                {
                    emptyReachedStageArray[i] = _reachedStage[i];
                }
            }

            _reachedStage = emptyReachedStageArray;
        }

        if (_reachedStage[StageIndex] == null)
        {
            _reachedStage[StageIndex] = new StageReached { stageNumber = StageIndex, amountOfTimesReached = 0};
        }

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
