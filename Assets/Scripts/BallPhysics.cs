using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallPhysics : MonoBehaviour
{
    
    // The starting 3D Velocity throw vector.
    public Vector3 m_V0;
    
    // The starting playable position of the lane.
    [SerializeField]
    Vector3 m_startPosition;

    // The predefined curved graph that will detrmine how much will the ball turn and spin over time.
    [SerializeField]
    AnimationCurve m_turnAndSpinOverTime;
    
    // The rigid body component to do physics with.
    Rigidbody m_body;

    // The dot product that will determine the orientation and speed of the spin
    [SerializeField]
    float m_spinDotP;

    // Force Curve multiplier for controlling how much will the ball curve over time based on spin.
    [SerializeField]
    float m_curveMult;
    
    // This is a maximum dot product value that I came up with to see if the ball should spin and curve.
    const float m_maxDotPValue = 0.09668773f;
    
    void Awake()
    {
        m_body = GetComponent<Rigidbody>();
    }
    
    void OnEnable()
    {
        // Clamp initial velocity so that the ball does not stop rolling before it reaches the end,
        // or so that it does not go flying off.
        m_V0.z = Mathf.Clamp(m_V0.z, 6.0f, 7.5f);
        m_V0.y = m_V0.z / 4;
        m_body.velocity = m_V0;
    }

    // Using FixedUpdate because this is for physics calculations
    void FixedUpdate()
    {
        // Calculating how much the ball is spinning, depending on a predefined curved graph
        float ballspin = ((transform.position.z - m_startPosition.z) > 0) && (m_spinDotP <= m_maxDotPValue) ?
                            Mathf.Sign(m_spinDotP) *
                            m_turnAndSpinOverTime.Evaluate(Mathf.Abs(transform.position.z - m_startPosition.z))
                            : 0;

        // Based on the initial throw angle, curve the ball
        m_body.AddForce(-m_spinDotP * m_curveMult, 0, 0);

        transform.Rotate(0, 0, ballspin, Space.World);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Checking when to start considering for spin.
        switch(collision.gameObject.tag) {
            case "Start of Lane":
                // Set the position of the starting play area to determine distance.
                m_startPosition = collision.transform.position;

                // Set the spin direction and velocity multiplier
                m_spinDotP = Vector2.Dot(collision.transform.right, new Vector2(m_V0.x, m_V0.z).normalized);

                // Ignore this initial collision with collider
                Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider);
                
                // Reset the current velocity to be the initial throw velocity.
                m_body.velocity = new Vector3(m_V0.x, m_body.velocity.y, m_V0.z);
            break;
        }
    }
}
