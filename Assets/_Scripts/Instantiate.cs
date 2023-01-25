using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Instantiate : MonoBehaviour
{
    public Object player1;
    public Object player2;

    public PlayerInputManager manager;

    public void SwapPrefab()
    {
        manager.playerPrefab = (GameObject)player2;

        if (manager.playerCount > 1)
        {
            manager.DisableJoining();
        }
    }
}
