using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Tutorial2
{
    public class GameManager : MonoBehaviour
    {
        public UnityEvent<int, GameObject> onGameStart;

        private PlayerActionsTest _playerActions;
        private InputAction _startActionEvent;
        private InputAction _startIdleAnimation;
        private InputAction _startAttackAnimation;
        private InputAction _startRunAnimation;

        public Animator enemyAnimatorRef;
        
        private static GameManager _instance;
        
        public static GameManager Instance => _instance;

        void Awake()
        {
            _instance = this;

            _playerActions = new PlayerActionsTest();
            _startActionEvent = _playerActions.Player2.StartActionEvent;

            _startIdleAnimation = _playerActions.Player2.Idle;
            _startAttackAnimation = _playerActions.Player2.Attack;
            _startRunAnimation = _playerActions.Player2.Run;

            _startActionEvent.Enable();
            _startIdleAnimation.Enable();
            _startAttackAnimation.Enable();
            _startRunAnimation.Enable();
        }

        void OnEnable()
        {
            _startActionEvent.performed += StartGame;
            _startAttackAnimation.performed += StartAttackAnimation;
            _startIdleAnimation.performed += StartIdleAnimation;
            _startRunAnimation.performed += StartRunAnimation;
        }

        private void StartGame(InputAction.CallbackContext context)
        {
            Debug.Log(onGameStart.GetPersistentEventCount());
            onGameStart?.Invoke(1, FindObjectOfType<PlayerController>().gameObject);
        }

        private void StartIdleAnimation(InputAction.CallbackContext context)
        {
            enemyAnimatorRef.SetTrigger("Idle");
        }
        
        private void StartAttackAnimation(InputAction.CallbackContext context)
        {
            enemyAnimatorRef.SetTrigger("Attack");
        }
        
        private void StartRunAnimation(InputAction.CallbackContext context)
        {
            enemyAnimatorRef.SetTrigger("Run");
        }

        void OnDisable()
        {
            onGameStart.RemoveAllListeners();
            _startActionEvent.performed -= StartGame;
        }
    }
}