using UnityEngine;
using FMODUnity;

public class PlayFMODSound : MonoBehaviour
{
    [SerializeField]
    private EventReference soundEvent;

    public void PlaySound()
    {
        RuntimeManager.PlayOneShot(soundEvent);
    }
}