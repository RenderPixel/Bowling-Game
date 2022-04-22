using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleShop : MonoBehaviour
{
    // Start is called before the first frame update
   public GameObject CurrentPanel;
   public GameObject OtherPanel;

   // UI Button logic
   public void ChangePanel(){
       if(CurrentPanel != null && OtherPanel != null){
           OtherPanel.SetActive(true);
           CurrentPanel.SetActive(false);
           
       }
   }
}
