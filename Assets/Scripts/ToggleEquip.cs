using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleEquip : MonoBehaviour
{
   public GameObject CurrentButton;
   public GameObject OtherButton;

   // UI Button logic
   public void ChangeButton(){
       if(CurrentButton != null && OtherButton != null){
           OtherButton.SetActive(true);
           CurrentButton.SetActive(false);           
       }
   }
}
