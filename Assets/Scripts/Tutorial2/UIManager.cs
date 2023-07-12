using UnityEngine;

namespace Tutorial2
{
    public class UIManager : MonoBehaviour
    {
        public void OnLevelStart(int level, GameObject player)
        {
            Debug.Log($"Level Start {level} {player.name}");
        }
    }
}