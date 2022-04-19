using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class BallThrowingController : MonoBehaviour
{

    // Camera to use
    [SerializeField]
    Camera m_camera;
    
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

    void OnEnable()
    {
        // Enable the corret touch inputs.
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable();
        
        // Add the correct function to the right even listener.
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += OnTouchScreen;

        // Resetting the panning bool to make sure we can select the ball if we wanted to.
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += (finger) => m_panning = false;
    }

    void OnDisable()
    {
        // Unsubscribe the move ball function first.
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp -= OnTouchScreen;

        // Disable all touch inputs to make sure not errors happen after the ball is thrown.
        EnhancedTouchSupport.Disable();
        TouchSimulation.Disable();
    }

    void Start()
    {
        m_initialZOffset = -transform.position.z;
    }

    void Update()
    {
        
    }

    public void OnTouchScreen(Finger finger)
    {
        // Figure out the velocity at which the finger is dragging along the screen.
        Vector2 fingerDelta = finger.screenPosition - m_previousFingerPos;
        Vector2 fingerDxDtDyDt = fingerDelta / Time.deltaTime;

        // Create a ray to check if we are effectively dragging the ball across the screen.
        Ray cameraOrHolderRay = m_camera.ScreenPointToRay(finger.screenPosition);

        // The 7 at the end is to indicate the physics layer mask that we are using, which is the holder mask.
        if(Physics.Raycast(cameraOrHolderRay, float.PositiveInfinity, 7) && m_panning == false)
            MoveBall(finger);

        // If not, then pan the camera left or right.
        else
        {
            m_panning = true;

            m_camera.transform.Rotate(0, -fingerDxDtDyDt.x / 700, 0);

            Vector3 angles = m_camera.transform.eulerAngles;
            float y = ((angles.y + 540) % 360) - 180 - OFFSET;

            // If we are outside the range
            if(Mathf.Abs(y) > REL_RANGE)
            {
                angles.y = REL_RANGE * Mathf.Sign(y) + OFFSET;
                m_camera.transform.eulerAngles = angles;
            }
        }

        // Update the previous finger position.
        m_previousFingerPos = finger.screenPosition;
    }

    void MoveBall(Finger finger)
    {
        // Create a mathematical plane to map the ball position.
        Plane plane = new Plane(m_camera.transform.forward.normalized, m_initialZOffset);

        float distance;
        
        // Create a fingerRay from the position of where the finger is on the screen.
        Ray fingerRay = m_camera.ScreenPointToRay(finger.screenPosition);

        // Set a dummy world position value.
        Vector3 worldPosition = Vector3.zero;

        // If a plane-fingerRay intersection did happen, get the world position of it.
        if(plane.Raycast(fingerRay, out distance))
            worldPosition = fingerRay.GetPoint(distance);
        
        // Convert the world position into view position.
        Vector3 viewPortPosition = m_camera.WorldToViewportPoint(worldPosition);

        // Clamp the view position to be between 0 and 1, which are the screen bounds
        // that way, it never leaves the screen.
        viewPortPosition.x = Mathf.Clamp01(viewPortPosition.x);
        viewPortPosition.y = Mathf.Clamp01(viewPortPosition.y);

        // Convert the view position back to world position.
        transform.position = m_camera.ViewportToWorldPoint(viewPortPosition);
    }
}
