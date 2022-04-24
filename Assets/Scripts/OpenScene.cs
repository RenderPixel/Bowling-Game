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
   public void OpenStageOne(){
      SceneManager.LoadScene("1st stage");
   }
   public void OpenStageTwo(){
      SceneManager.LoadScene("2nd stage");
   }
   public void OpenStageThree(){
      SceneManager.LoadScene("3rd stage");
   }
   public void OpenStageFour(){
      SceneManager.LoadScene("4th stage");
   }
   public void OpenStageFive(){
      SceneManager.LoadScene("5th stage");
   }
}
