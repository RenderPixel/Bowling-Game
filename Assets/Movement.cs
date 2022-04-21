using UnityEngine;

public class Movement : MonoBehaviour
{
    public GameObject ball;
    public Rigidbody rb;
    //Movement Scipts to help test
    public float fowardForce = 10f;
    public float sideForce = 5f;

    private Vector3 ballPos;

    void Start() {
        //Grabbing the Postion of the ball
        ballPos = GameObject.FindGameObjectWithTag("Ball").transform.position;  
    }

    // When working with the physics system used FixedUpdate()
    void FixedUpdate(){
        if(ball != null){
            if (Input.GetKey("w"))
            {
                rb.AddForce(0, 0, fowardForce * Time.deltaTime);
            }
            if (Input.GetKey("s"))
            {
                rb.AddForce(0, 0, -fowardForce * Time.deltaTime);
            }

            // Move the cube right
            if (Input.GetKey("d"))
            {
                rb.AddForce(sideForce * Time.deltaTime, 0, 0);
            }
            // move the cube left
            if (Input.GetKey("a"))
            {
                rb.AddForce(-sideForce * Time.deltaTime, 0, 0);
            }
            //Reset the round
            if (Input.GetKey("r"))
            {
                ///Move back to the starting location
            }
        }
    }

    public float timeDespawn = 5;
    private bool collisionOccured = false;

    void OnCollisionEnter(Collision col)
    {
        if (collisionOccured)
            return;
        if (col.gameObject.name == "OoB")
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
