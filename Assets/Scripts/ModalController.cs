using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalController : MonoBehaviour
{
   public GameObject Modal;

   public void OpenModal() {
       if(Modal != null){
           Modal.SetActive(true);
       }
   }

   public void CloseModal() {
       if(Modal != null){
           Modal.SetActive(false);
       }
   }
}