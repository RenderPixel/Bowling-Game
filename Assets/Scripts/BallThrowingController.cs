using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Cinemachine;

[RequireComponent(typeof(Animator))]
public class BallThrowingController : MonoBehaviour
{
    // Camera to use
    [SerializeField]
    CinemachineVirtualCamera m_camera;

    // An animator to controller the transition between cameras
    [SerializeField]
    Animator m_animator;
    
    // Ball prefab to use when instantiating a new ball when the round resets
    [SerializeField]
    GameObject m_ballPrefab;
    
    // An initial offset to keep the ball on screen while it is being held.
    float m_initialZOffset;

    // The previous screen position of the active finger interacting on screen.
    Vector2 m_previousFingerPos = Vector2.zero;

    // Minimum and maximum angles of camera panning.
    const float MIN_ANGLE = -10f;
    const float MAX_ANGLE = 10f;
    
    // Relative range of panning motion
    const float REL_RANGE = (MAX_ANGLE - MIN_ANGLE) / 2f;

    // Offset between angles
    const float OFFSET = MAX_ANGLE - REL_RANGE;
    
    // A boolean used to check if we are panning the camera, so if we overlap with the ball
    // as we are doing so, it will not switch to dragging the ball.
    bool m_panning = false;

    // Overall finger velocity vector
    Vector2 m_fingerDxDtDyDt;

    // A boolean used to check if, we are holding the ball, and we have enough velocity,
    // the ball will be thrown.
    bool m_ballHeld;

    void OnEnable()
    {
        // Enable the corret touch inputs.
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable();
        
        // Add the correct function to the right even listener.
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += OnTouchScreen;

        // Resetting the panning bool to make sure we can select the ball if we wanted to.
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += (finger) => m_panning = false;

        // Subscribing the throw ball function when the finger is lifted up
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += ThrowBall;
    }

    void OnDisable()
    {
        // Switch to the follow Camera
        m_animator.Play("Follow Camera");

        // Unsubscribe all previous finger functions
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove -= OnTouchScreen;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp -= ThrowBall;

        // Disable all touch inputs to make sure not errors happen after the ball is thrown.
        EnhancedTouchSupport.Disable();
        TouchSimulation.Disable();
    }

    void Start()
    {
        m_initialZOffset = -transform.position.z;
    }

    public void OnTouchScreen(Finger finger)
    {
        // Figure out the velocity at which the finger is dragging along the screen.
        Vector2 fingerDelta = finger.screenPosition - m_previousFingerPos;
        m_fingerDxDtDyDt = fingerDelta / Time.deltaTime;

        // Create a ray to check if we are effectively dragging the ball across the screen.
        Ray cameraOrHolderRay = Camera.main.ScreenPointToRay(finger.screenPosition);

        // The 7 at the end is to indicate the physics layer mask that we are using, which is the holder mask.
        if(Physics.Raycast(cameraOrHolderRay, float.PositiveInfinity, LayerMask.GetMask("Holder")) && m_panning == false)
            MoveBall(finger);

        // If not, then pan the camera left or right.
        else
            PanCamera(m_fingerDxDtDyDt.x);

        // Update the previous finger position.
        m_previousFingerPos = finger.screenPosition;
    }

    void MoveBall(Finger finger)
    {
        // Setting the bool that says we are moving the ball
        m_ballHeld = true;

        // Create a mathematical plane to map the ball position.
        Plane plane = new Plane(m_camera.transform.forward.normalized, m_initialZOffset);

        float distance;
        
        // Create a fingerRay from the position of where the finger is on the screen.
        Ray fingerRay = Camera.main.ScreenPointToRay(finger.screenPosition);

        // Set a dummy world position value.
        Vector3 worldPosition = Vector3.zero;

        // If a plane-fingerRay intersection did happen, get the world position of it.
        if(plane.Raycast(fingerRay, out distance))
            worldPosition = fingerRay.GetPoint(distance);
        
        // Convert the world position into view position.
        Vector3 viewPortPosition = Camera.main.WorldToViewportPoint(worldPosition);

        // Clamp the view position to be between 0 and 1, which are the screen bounds
        // that way, it never leaves the screen.
        viewPortPosition.x = Mathf.Clamp01(viewPortPosition.x);
        viewPortPosition.y = Mathf.Clamp01(viewPortPosition.y);

        // Convert the view position back to world position.
        transform.position = Camera.main.ViewportToWorldPoint(viewPortPosition);
    }
    
    Vector3 RemapCameraAngles()
    {
        Vector3 angles = m_camera.transform.eulerAngles;
        float y = ((angles.y + 540) % 360) - 180 - OFFSET;

        // If we are outside the range
        if(Mathf.Abs(y) > REL_RANGE)
            angles.y = REL_RANGE * Mathf.Sign(y) + OFFSET;

        return angles;
    }
    
    void PanCamera(float fingerXVel)
    {
        m_panning = true;

        m_camera.transform.Rotate(0, -fingerXVel / 700, 0);

        m_camera.transform.eulerAngles = RemapCameraAngles();
    }

    void ThrowBall(Finger finger)
    {
        // Check if we are holding the ball, and we have enough finger velocity
        if(m_ballHeld && m_fingerDxDtDyDt.y > 200)
        {
            // Unity's Euler Angles are only between 0 and 360 inclusive,
            // so if we want to have negative angles, we have to do a simple remapping.
            float xAngle = RemapCameraAngles().y;
            xAngle = xAngle >= 350 ? xAngle - 360 : xAngle;
 
            // Give the child (ball) its initial velocity
            GetComponentInChildren<BallPhysics>().m_V0 =
                                                    new Vector3((xAngle / 10.1f) + (m_fingerDxDtDyDt.x / 600f),
                                                                m_fingerDxDtDyDt.y / 600f, m_fingerDxDtDyDt.y / 300);
           
            // Enable gravity for the actual ball
            GetComponentInChildren<Rigidbody>().useGravity = true;

            // Start the ball physics script
            GetComponentInChildren<BallPhysics>().enabled = true;

            // Free the ball from being a child of this
            transform.DetachChildren();

            // Disable this script
            enabled = false;
        }
    }
}
