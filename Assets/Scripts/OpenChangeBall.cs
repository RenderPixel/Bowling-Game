using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChangeBall : MonoBehaviour
{
   public GameObject CurrentCanvas;
   public GameObject NextCanvas;

   // UI Button logic
   public void OpenChange(){
        NextCanvas.SetActive(true);
        CurrentCanvas.SetActive(false);
   }
}
