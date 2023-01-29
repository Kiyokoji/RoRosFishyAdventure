using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class TitleMusic : MonoBehaviour
{
    public StudioEventEmitter eventEmitter;
    
    public EventReference titleTrack;
    private EventInstance titleMusic;

    public float musicDelay = 3f;

    private void Awake()
    {
        //titleMusic = FMODUnity.RuntimeManager.CreateInstance(titleTrack);

        StartCoroutine(MusicStartDelay());
    }
    
    
    public IEnumerator MusicStartDelay()
    {
        yield return new WaitForSeconds(musicDelay);
        //titleMusic.start();
        eventEmitter.Play();
    }
}
