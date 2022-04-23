using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    // Singleton instance setup
    private static PlayerInventory instance = null;
    
    public static PlayerInventory Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<PlayerInventory>();

                if(instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = "Player Inventory";
                    instance = go.AddComponent<PlayerInventory>();
                }
            }

            return instance;
        }
    }

    // Default ball object
    [SerializeField]
    GameObject m_defaultBall;

    // Rezizeable inventory dictionary
    // It has a look up name, the ball gameobject, and a boolean to check if the ball is being used.
    Dictionary<string, (GameObject Ball, bool isUsed)> m_balls = new Dictionary<string, (GameObject, bool)>(1);

    public Dictionary<string, (GameObject, bool)> balls
    {
        get
        {
            return m_balls;
        }
    }

    // The currently set ball.
    string m_currentBall;

    public string currentBall
    {
        get
        {
            return m_currentBall;
        }
    }

    void Awake()
    {

        if(instance == null)
        {
            // Setting the first ball in the inventory to be the default one
            m_balls.Add(m_defaultBall.name, (m_defaultBall, true));
            m_currentBall = m_defaultBall.name;

            // Make this gameobject exist in all the scenes
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(gameObject);

    }

    public void Setball(string name)
    {
        // Set the boolean of the current ball before being switched to false.
        m_balls[m_currentBall] = (m_balls[m_currentBall].Ball, false);

        // Set the new current ball in inventory and name.
        m_balls[name] = (m_balls[name].Ball, true);
        m_currentBall = name;
    }

    public void AddBall(GameObject ballFromStore)
    {
        m_balls.Add(ballFromStore.name, (ballFromStore, false));
    }
}
