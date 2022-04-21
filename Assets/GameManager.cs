// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public GameObject ball;

    public GameObject prefab;
    
    // int loc = 0;
    // bool[] ballReset;
    
    int score = 0;

    //List of the 10 Pins
    GameObject[] pins;

    //Keeping track of the knocked 
    bool[] PinsKnockedOver;
    bool reset = false;

    //Holding starting location for the ball
    private Vector3 ballPos;
    private Quaternion ballRot;

    // Start is called before the first frame update
    void Start(){
        pins = GameObject.FindGameObjectsWithTag("Pin");
        PinsKnockedOver = new bool[pins.Length];

        //Grabbing the Postion of the ball
        ballPos = GameObject.FindGameObjectWithTag("Ball").transform.position;
        ballRot = GameObject.FindGameObjectWithTag("Ball").transform.rotation;
    }

    

    // Update is called once per frame
    void FixedUpdate(){
        if(ball != null){
            if (ball.transform.position.z > 60){
                PinsKnocked();
            }
        }
        if (ball == null)
        {
            if(reset == false){
                for (int i = 0; i < PinsKnockedOver.Length; i++)
                {
                    if (PinsKnockedOver[i] == true && pins[i] != null)
                    {
                        Destroy(pins[i]);
                    }
                }
                Instantiate(prefab, ballPos, ballRot);
                reset = true;
            }
        }
    }

    void PinsKnocked(){
        for(int i = 0; i < pins.Length; i++){
            if(pins[i] != null){
                if (pins[i].transform.eulerAngles.x > 30 && pins[i].transform.eulerAngles.x < 355
                    && PinsKnockedOver[i] == false){
                    score++;
                    PinsKnockedOver[i] = true;
                }
            }
        }
        Debug.Log("Your score is " + score);
    }
}
