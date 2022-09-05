using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue;
    [SerializeField] private Slider volumeSlider;

    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public AudioSource currentAudio;

    private TextMeshPro resetButton;

    private PlayerController player;

    private void Start()
    {
        currentAudio = MusicPlayer.instance.GetCurrentTrack();

        volumeTextValue.text = PlayerPrefs.GetFloat("masterVolume").ToString("0.0");
        volumeSlider.value = PlayerPrefs.GetFloat("masterVolume");

        //if (!PhotonNetwork.IsMasterClient) resetButton.faceColor = new Color32(100, 100, 100, 255);
        
        StartCoroutine(LateStart());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
        {
            if (gameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        StartCoroutine(StartFade(currentAudio, 1f, PlayerPrefs.GetFloat("masterVolume")));
        gameIsPaused = false;
        player.canMove = true;
        player.canUseFlashlight = true;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        StartCoroutine(StartFade(currentAudio, .7f, 0.05f));
        gameIsPaused = true;
        player.canMove = false;
        player.canUseFlashlight = false;
        if (player.flashLight.FlashlightOn) player.flashLight.GetComponent<PhotonView>().RPC("ToggleFlashlight", RpcTarget.All);
    }

    public void QuitGame()
    {
        Resume();
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
    }

    public void Reset()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameManager.instance.GetComponent<PhotonView>().RPC("Restart", RpcTarget.All);
        }
    }

    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
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
    
    private IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.05f);
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach (var t in players)
        {
            if (t.CompareTag("Player1"))
            {
                if (t.isMe) player = t;
            }

            if (t.CompareTag("Player2"))
            {
                if (t.isMe) player = t;
            }
        }
    }
}
