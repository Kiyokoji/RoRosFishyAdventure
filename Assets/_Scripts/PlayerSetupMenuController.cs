using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetupMenuController : MonoBehaviour
{
    private int PlayerIndex;

    [SerializeField] private Button readyButton;

    //so button press is not immediately registered when enabling menu
    private float ignoreInputTime = 1.5f;
    private bool inputEnabled;

    public void SetPlayerIndex(int playerIndex)
    {
        PlayerIndex = playerIndex;
        ignoreInputTime = Time.time + ignoreInputTime;
    }

    void Update()
    {
        if (Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

    public void ReadyPlayer(int player)
    {
        PlayerConfigManager.Instance.ReadyPlayer(player);
        readyButton.gameObject.SetActive(false);
    }
}
