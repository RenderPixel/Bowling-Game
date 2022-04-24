using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public GameObject ball;

    public GameObject prefab;

    public GameObject pinPre;
    
    int score = 1;

    [SerializeField]
    GameObject ballHolder;

    [SerializeField]
    CinemachineVirtualCamera followCamera;

    //List of the 10 Pins
    [SerializeField]
    GameObject[] pins = new GameObject[10];

    GameObject[] pinsPrefab = new GameObject[10];

    GameObject prefabInst;

    //Keeping track of the knocked 
    bool[] PinsKnockedOver = new bool[10];
    bool reset = false;

    private Quaternion ballRot;

    [SerializeField]
    private Vector3[] pinPos = new Vector3[10];

    [SerializeField]
    private Quaternion[] pinRot = new Quaternion[10];

    // Start is called before the first frame update
    void Start(){
        prefab = PlayerInventory.Instance.balls[PlayerInventory.Instance.currentBall].Item1;

        pins = GameObject.FindGameObjectsWithTag("Pin");
        PinsKnockedOver = new bool[pins.Length];

        //Grabbing the Postion of the ball
        ballRot = GameObject.FindGameObjectWithTag("Ball").transform.rotation;

        for (int i = 0; i < pins.Length; i++){
            pinPos[i] = pins[i].GetComponent<Transform>().position;
            pinRot[i] = pins[i].GetComponent<Transform>().rotation;
        }
    }

    public int bowlingLoop = 3;
    private int bLoop = 0;

    void FixedUpdate(){
        if(bLoop != bowlingLoop){
            //First Ball is active
            if (ball != null)
            {
                //First instance of the game no need for the prefab scoring method
                if (ball.transform.position.z > 10)
                    PinsKnocked();
            }
            // First Ball is destroyed
            else if (ball == null)
            {
                //First Part of the next stage
                if (reset == false)//right here 
                {
                    if (prefabInst != null)
                    {
                        //Scoring Functions
                        if (prefabInst.transform.position.z > 10)
                            PinsKnocked();
                    }
                    else if (prefabInst == null)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            if (PinsKnockedOver[i] == true && pins[i] != null)
                                Destroy(pins[i]);
                            else if (PinsKnockedOver[i] == true && pinsPrefab[i] != null)
                                Destroy(pinsPrefab[i]);
                        }
                        for(int i = 0; i < 10; i++){
                            //Reseting pins locations
                            if(PinsKnockedOver[i]==false && pinsPrefab[i]==null){
                                pins[i].transform.position = pinPos[i];
                                pins[i].transform.rotation = pinRot[i];
                            }
                            else if(PinsKnockedOver[i]==false && pins[i]==null){
                                pinsPrefab[i].transform.position = pinPos[i];
                                pinsPrefab[i].transform.rotation = pinRot[i];
                            }
                        }

                        prefabInst = Instantiate(prefab, ballHolder.transform.position, Quaternion.Euler(Vector3.zero));
                        prefabInst.transform.parent = ballHolder.transform;
                        followCamera.Follow = prefabInst.transform;
                        reset = true;//maybe issue
                    }
                }
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
                            }
                            else if(pinsPrefab[j] != null){
                                Destroy(pinsPrefab[j]);
                            }
                        }
                        for (int i = 0; i < 10; i++)
                            pinsPrefab[i] = Instantiate(pinPre, pinPos[i], pinRot[i]);
                    }

                    prefabInst = Instantiate(prefab, ballHolder.transform.position, Quaternion.Euler(Vector3.zero));
                    prefabInst.transform.parent = ballHolder.transform;
                    followCamera.Follow = prefabInst.transform;
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
                if (pins[i].transform.eulerAngles.z > 30 && pins[i].transform.eulerAngles.z < 355
                    && PinsKnockedOver[i] == false){
                    score++;
                    PinsKnockedOver[i] = true;
                }
            }
            else if (pinsPrefab[i] != null){
                if (pinsPrefab[i].transform.eulerAngles.z > 30 && pinsPrefab[i].transform.eulerAngles.z < 355
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
