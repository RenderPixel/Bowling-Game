// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public GameObject ball;

    // [SerializeField]
    public GameObject prefab;


    // [SerializeField]
    public GameObject pinPre;
    
    // int loc = 0;
    // bool[] ballReset;
    
    int score = 0;

    [SerializeField]
//List of the 10 Pins
    GameObject[] pins = new GameObject[10];

    GameObject[] pinsPrefab = new GameObject[10];

    GameObject prefabInst;

    //Keeping track of the knocked 
    bool[] PinsKnockedOver = new bool[10];
    bool reset = false;
    // int reset = 0;

    //Holding starting location for the ball
    private Vector3 ballPos;
    private Quaternion ballRot;

    [SerializeField]
    private Vector3[] pinPos = new Vector3[10];

    [SerializeField]
    private Quaternion[] pinRot = new Quaternion[10];

    // Start is called before the first frame update
    void Start(){
        pins = GameObject.FindGameObjectsWithTag("Pin");
        PinsKnockedOver = new bool[pins.Length];

        //Grabbing the Postion of the ball
        ballPos = GameObject.FindGameObjectWithTag("Ball").transform.position;
        ballRot = GameObject.FindGameObjectWithTag("Ball").transform.rotation;

        for (int i = 0; i < pins.Length; i++){
            pinPos[i] = pins[i].GetComponent<Transform>().position;
            pinRot[i] = pins[i].GetComponent<Transform>().rotation;
        }
    }

    public int bowlingLoop = 3;
    private int bLoop = 0;

    // private bool pinDes = false;

    // Update is called once per frame
    void FixedUpdate(){
        if(bLoop != bowlingLoop){
            //First Ball is active
            if (ball != null)
            {
                //First instance of the game no need for the prefab scoring method
                if (ball.transform.position.z > 60)
                {
                    PinsKnocked();
                    // reset = true;
                }
                // reset = 1;
            }
            // First Ball is destroyed
            else if (ball == null)
            {
                //First Part of the next stage
                // if (reset == false)//right here 
                if (reset == false)//right here 
                {
                    if (prefabInst != null)
                    {
                        //Scoring Functions
                        if (prefabInst.transform.position.z > 60){
                            // Debug.Log("YESASLDJKAJKSDH");
                            PinsKnocked();
                        }
                        
                    }
                    else if (prefabInst == null)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            if (PinsKnockedOver[i] == true && pins[i] != null)
                            {
                                Destroy(pins[i]);
                            }
                            else if (PinsKnockedOver[i] == true && pinsPrefab[i] != null)
                            {
                                Destroy(pinsPrefab[i]);
                            }
                        }
                        prefabInst = Instantiate(prefab, ballPos, ballRot);
                        reset = true;//maybe issue
                    }
                }
                // if (prefabInst != null)
                // {
                //     //Scoring Functions
                //     if (prefabInst.transform.position.z > 60)
                //     {
                //         // Debug.Log("YESASLDJKAJKSDH");
                //         PinsKnocked();
                //         reset =  true;
                //     }

                // }
                // else if (prefabInst == null)
                // {
                //     for (int i = 0; i < 10; i++)
                //     {
                //         if (PinsKnockedOver[i] == true && pins[i] != null)
                //         {
                //             Destroy(pins[i]);
                //         }
                //         else if (PinsKnockedOver[i] == true && pinsPrefab[i] != null)
                //         {
                //             Destroy(pinsPrefab[i]);
                //         }
                //     }
                //     prefabInst = Instantiate(prefab, ballPos, ballRot);
                //     reset = false;//maybe issue
                // }
                //The ball prefab is destroyed
                else if (prefabInst == null)
                {
                    //Where the actual reset happens
                    if (reset == true)
                    {
                        Debug.Log("yes");
                        PinsKnocked();
                        for (int i = 0; i < 10; i++)
                        {
                            if (PinsKnockedOver[i] == true && pins[i] != null){
                                Destroy(pins[i]);
                            }
                            else if(PinsKnockedOver[i] == true && pinsPrefab[i] != null){
                                Destroy(pinsPrefab[i]);
                            }
                        }
                        //Destroying all the pins before making new ones
                        for (int j = 0; j < 10; j++)
                        {
                            //Check to see if the pins are destroyed or not
                            PinsKnockedOver[j] = false;
                            Debug.Log(PinsKnockedOver[j]);
                            if (pins[j] != null){
                                Destroy(pins[j]);
                                // pinDes = true;
                            }
                            else if(pinsPrefab[j] != null){
                                Destroy(pinsPrefab[j]);
                            }
                        }
                        for (int i = 0; i < 10; i++)
                        {
                            // Quaternion.idnetity
                            pinsPrefab[i] = Instantiate(pinPre, pinPos[i], pinRot[i]);
                        }
                    }
                    prefabInst = Instantiate(prefab, ballPos, ballRot);
                    reset = false;
                    bLoop++;
                }
            }
        }
        if(bLoop == bowlingLoop)
            Debug.Log(score);
    }

    void PinsKnocked(){
        for(int i = 0; i < 10; i++){
            if(pins[i] != null){
                if (pins[i].transform.eulerAngles.x > 30 && pins[i].transform.eulerAngles.x < 355
                    && PinsKnockedOver[i] == false){
                    score++;
                    PinsKnockedOver[i] = true;
                }
            }
            else if (pinsPrefab[i] != null){
                if (pinsPrefab[i].transform.eulerAngles.x > 30 && pinsPrefab[i].transform.eulerAngles.x < 355
                    && PinsKnockedOver[i] == false)
                {
                    score++;
                    PinsKnockedOver[i] = true;
                }
            }
            Debug.Log("Your score is " + score);
        }
        
    }
}
