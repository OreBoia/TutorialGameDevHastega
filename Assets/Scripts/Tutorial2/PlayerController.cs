using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public UnityAction<int, GameObject> onPlayerSpawned;

    [SerializeField] private float hp = 100f;

    private static PlayerController instance;

    // Ensure that the GameManager is a Singleton
    public static PlayerController Instance
    { 
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;

        GameManager.Instance.onGameStart.AddListener(InitPlayer);
    }

    void OnEnable()
    {
        onPlayerSpawned += PlayerSpawned;
    }
    
    public void InitPlayer( int value, GameObject player)
    {
        onPlayerSpawned?.Invoke(1, this.gameObject);
    }

    public void PlayerSpawned(int value, GameObject player)
    {
        hp = 0f;
        Debug.Log($"Player Spawned with {hp} hp; Object name: {player.name}");
    }

    void OnDisable()
    {
        onPlayerSpawned -= PlayerSpawned;
    }
}