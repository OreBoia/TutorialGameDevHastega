using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Tutorial2
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float hp = 100f;
        private UnityAction<int, GameObject> _onPlayerSpawned;

        // Ensure that the GameManager is a Singleton
        private static PlayerController _instance;
        
        public static PlayerController Instance => _instance;

        private void Awake()
        {
            _instance = this;

            GameManager.Instance.onGameStart.AddListener(InitPlayer);
        }

        private void OnEnable()
        {
            _onPlayerSpawned += PlayerSpawned;
        }

        private void InitPlayer(int value, GameObject player)
        {
            _onPlayerSpawned?.Invoke(1, gameObject);
        }

        private void PlayerSpawned(int value, GameObject player)
        {
            hp = 0f;
            Debug.Log($"Player Spawned with {hp} hp; Object name: {player.name}");
        }
        
        private void OnDisable()
        {
            _onPlayerSpawned -= PlayerSpawned;
        }
    }
}