using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DebugLogger : MonoBehaviour
{
    [SerializeField] private Text textP1, textP2;

    private PlayerController player1, player2;

    private void Start()
    {
        StartCoroutine(LateStart());
    }

    private void Update()
    {
        if (player1 != null) textP1.text = "Player1 facingRight: " + player1.facingRight;
        if (player2 != null) textP2.text = "Player2 facingRight: " + player2.facingRight;
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(1f);
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach (var t in players)
        {
            if (t.CompareTag("Player1")) player1 = t;
            if (t.CompareTag("Player2")) player2 = t;
        }
    }
}
