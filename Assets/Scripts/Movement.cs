using UnityEngine;

public class Movement : MonoBehaviour
{
    // When working with the physics system used FixedUpdate()
    public float timeDespawn = 2;
    private bool collisionOccured = false;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Bound")
        {
            collisionOccured = true;
        }
    }

    void Update(){
        if (collisionOccured){
            if (timeDespawn > 0){
                timeDespawn -= Time.deltaTime;
            }
            else{
                Destroy(this.gameObject);
                timeDespawn += 5;
            }
        }
    }

    void OnDestroy() 
    {
        var ballHolder = GameObject.Find("Ball Holder");

        ballHolder.GetComponent<Animator>().SetTrigger("New Ball");
        ballHolder.GetComponent<BallThrowingController>().enabled = true;
    }
}
