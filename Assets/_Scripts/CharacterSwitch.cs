using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSwitch : MonoBehaviour
{
    private int index = 0;
    [SerializeField] private List<GameObject> players = new List<GameObject>();
    private PlayerInputManager manager;
    
    
    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<PlayerInputManager>();
        index = Random.Range(0, players.Count);
        manager.playerPrefab = players[index];
    }

    public void SwapPrefab(PlayerInput input)
    {
        index = Random.Range(0, players.Count);
        manager.playerPrefab = players[index];
    }
}
