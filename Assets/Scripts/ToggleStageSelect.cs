using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleStageSelect : MonoBehaviour
{
  public GameObject PanelNext;
  public GameObject PanelTwo;
  public GameObject PanelThree;
  public GameObject PanelFour;
  public GameObject PanelFive;



   // UI Button logic
   public void ChangeStagePanel(){
           PanelNext.SetActive(true);
           PanelTwo.SetActive(false);
           PanelThree.SetActive(false);
           PanelFour.SetActive(false);
           PanelFive.SetActive(false);



           
           
       
    }
}
