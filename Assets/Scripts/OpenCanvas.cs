using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCanvas : MonoBehaviour
{
   public GameObject CurrentCanvas;
   public GameObject NextCanvas;
   public GameObject OtherCanvas;

   // UI Button logic
   public void ChangeCanvas(){
       if(CurrentCanvas != null && NextCanvas != null && OtherCanvas != null){
           NextCanvas.SetActive(true);
           CurrentCanvas.SetActive(false);
           OtherCanvas.SetActive(false);
       }
   }
}
