using UnityEngine;

public class StageSelection : MonoBehaviour
{
    private GameDataSaver dataSaver = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var data = dataSaver.LoadGameData();
        if (data != null)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
