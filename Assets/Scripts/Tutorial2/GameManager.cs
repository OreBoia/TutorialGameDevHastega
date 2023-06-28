using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public UnityEvent<int, GameObject> onGameStart;

    private InputAction ia_startActionEvent;
    private PlayerActionsTest playerActions;

    private static GameManager instance;

    // Ensure that the GameManager is a Singleton
    public static GameManager Instance
    { 
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;

        playerActions = new PlayerActionsTest();
        ia_startActionEvent = playerActions.Player.StartActionEvent;

        ia_startActionEvent.Enable();

        ia_startActionEvent.performed += StartGame;
    }

    void OnEnable()
    {
        ia_startActionEvent.performed += StartGame;
    }

    void Start()
    {  
        // StartGame();
    }

    private void OnPlayerSpawned(int value, GameObject player)
    {
        Debug.Log($"Player Spawned {value} {player.name}");
    }

    public void StartGame(InputAction.CallbackContext context)
    {
        Debug.Log(onGameStart.GetPersistentEventCount());
        onGameStart?.Invoke(1, FindObjectOfType<PlayerController>().gameObject);
    }

    void OnDisable()
    {
        onGameStart.RemoveAllListeners();
        ia_startActionEvent.performed -= StartGame;
    }
}