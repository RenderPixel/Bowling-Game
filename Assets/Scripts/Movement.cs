using UnityEngine;

public class Movement : MonoBehaviour
{
    public GameObject ball;

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
                Destroy(ball);
                timeDespawn += 5;
            }
        }
    }
}
