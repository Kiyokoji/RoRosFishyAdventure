using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip menuMusic;

    [SerializeField]
    private AudioClip levelMusic;

    [SerializeField]
    private AudioClip floodedMusic;

    [SerializeField]
    private AudioClip endMusic;

    [SerializeField]
    private AudioSource source;

    public static MusicPlayer instance;

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
            return;
        }
    }

    public void Start()
    {
        source.volume = PlayerPrefs.GetFloat("masterVolume");
        //FindObjectOfType<AudioManager>().Play("MenuTheme");
        // If the game starts in a menu scene, play the appropriate music

    }

    //Menu Music
    public void PlayMenuMusic()
    {
        if (instance != null)
        {
            if (instance.source != null)
            {
                StartCoroutine(MenuMusic());
            }
        }
        else
        {
            Debug.LogError("Unavailable MusicPlayer component");
        }
    }
      
    //Game Music
    public void PlayGameMusic()
    {
        if (instance != null)
        {
            if (instance.source != null)
            {
                StartCoroutine(LevelMusic());
            }
        }
        else
        {
            Debug.LogError("Unavailable MusicPlayer component");
        }
    }

    //Flooded Music
    public void PlayFloodedMusic()
    {
        if (instance != null)
        {
            if (instance.source != null)
            {
                StartCoroutine(FloodedMusic());
            }
        }
        else
        {
            Debug.LogError("Unavailable MusicPlayer component");
        }
    }

    public void PlayEndMusic()
    {
        if (instance != null)
        {
            if (instance.source != null)
            {
                StartCoroutine(EndMusic());
            }
        }
        else
        {
            Debug.LogError("Unavailable MusicPlayer component");
        }
    }

    public AudioSource GetCurrentTrack()
    {
        return instance.source;
    }

    public IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    public IEnumerator LevelMusic()
    {
        StartCoroutine(StartFade(instance.source, 1, 0f));
        yield return new WaitForSeconds(1f);
        instance.source.Stop();
        instance.source.clip = instance.levelMusic;
        instance.source.Play();
        StartCoroutine(StartFade(instance.source, 1, PlayerPrefs.GetFloat("masterVolume")));
    }

    public IEnumerator MenuMusic()
    {
        StartCoroutine(StartFade(instance.source, 1, 0f));
        yield return new WaitForSeconds(1f);
        instance.source.Stop();
        instance.source.clip = instance.menuMusic;
        instance.source.Play();
        StartCoroutine(StartFade(instance.source, 1, PlayerPrefs.GetFloat("masterVolume")));
    }

    public IEnumerator FloodedMusic()
    {
        StartCoroutine(StartFade(instance.source, 1, 0f));
        yield return new WaitForSeconds(1f);
        instance.source.Stop();
        instance.source.clip = instance.floodedMusic;
        instance.source.Play();
        StartCoroutine(StartFade(instance.source, 1, PlayerPrefs.GetFloat("masterVolume")));
    }

    public IEnumerator EndMusic()
    {
        StartCoroutine(StartFade(instance.source, 1, 0f));
        yield return new WaitForSeconds(1f);
        instance.source.Stop();
        instance.source.clip = instance.endMusic;
        instance.source.Play();
        StartCoroutine(StartFade(instance.source, 1, PlayerPrefs.GetFloat("masterVolume")));
    }

    public void StopPlaying()
    {
        instance.source.Stop();
    }
}
