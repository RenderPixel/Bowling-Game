using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class OpenScene : MonoBehaviour
{

   // UI changing between scenes
   public void OpenMenu(){
      SceneManager.LoadScene("Menu");
   }
   public void OpenBowlingLane(){
      SceneManager.LoadScene("Bowling Lane");
   }
}
