using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using FMODUnity;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class LevelMusic : MonoBehaviour
{
    public EventReference levelMusic;
    private EventInstance levelMusicInstance;

    public void Stop()
    {
        levelMusicInstance.stop(STOP_MODE.ALLOWFADEOUT);
    }

    public void Play()
    {
        levelMusicInstance.start();
    }
    
    void Start()
    {
        levelMusicInstance = FMODUnity.RuntimeManager.CreateInstance(levelMusic);
    }
}
