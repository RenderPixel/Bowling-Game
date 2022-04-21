using UnityEngine;

public class HitDetection : MonoBehaviour{
    public Rigidbody rb;

    // //hit detection
    // private void OnTriggerEnter(Collider other) {
    //     if(rb.rotation.z > 70){
    //         Debug.Log("+1 Score");
    //     }
    // } 

    // private bool collisionOccured = false;
    // private bool scored = false;

    // void OnCollisionEnter(Collision col) {
    //     // if(collisionOccured)
    //     //     return;
    //     // if(col.gameObject.name == "BowlingBall"){
    //     //     Debug.Log("Hit detected!");
    //     //     collisionOccured = true;
    //     // }

    //     if(scored)
    //         return;
    //     if(rb.rotation.z < 80.0f ){
    //         Debug.Log("+1 Score");
    //         scored = true;
    //     }
    // }
}
