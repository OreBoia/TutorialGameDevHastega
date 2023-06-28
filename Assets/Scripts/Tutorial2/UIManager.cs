using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class UIManager : MonoBehaviour
{
   public void OnLevelStart(int level, GameObject player)
   {
       Debug.Log($"Level Start {level} {player.name}");
   }
}   