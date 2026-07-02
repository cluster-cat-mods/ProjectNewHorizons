using UnityEngine;
using FMODUnity;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private StudioEventEmitter emitter;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        emitter = GetComponent<StudioEventEmitter>();
    }

    private void Start()
    {
        if (!emitter.IsPlaying())
        {
            emitter.Play();
        }
    }

    public void SetParameter(string parameterName, float value)
    {
        emitter.SetParameter(parameterName, value);
    }
}