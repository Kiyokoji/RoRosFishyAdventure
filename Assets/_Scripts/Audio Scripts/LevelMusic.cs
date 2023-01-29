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

    public float musicStartDelay = 5f;

    public void Stop()
    {
        levelMusicInstance.stop(STOP_MODE.ALLOWFADEOUT);
    }

    public void Play()
    {
        StartCoroutine(MusicStartDelay());
    }
    
    void Start()
    {
        levelMusicInstance = FMODUnity.RuntimeManager.CreateInstance(levelMusic);
    }

    public IEnumerator MusicStartDelay()
    {
        yield return new WaitForSeconds(musicStartDelay);
        levelMusicInstance.start();
    }
}
