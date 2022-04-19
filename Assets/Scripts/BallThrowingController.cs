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

    void OnEnable()
    {
        // Enable the corret touch inputs.
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable();
        
        // Add the correct function to the right even listener.
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += OnMoveBall;
    }

    void OnDisable()
    {
        // Unsubscribe the move ball function first.
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp -= OnMoveBall;

        // Disable all touch inputs to make sure not errors happen after the ball is thrown.
        EnhancedTouchSupport.Disable();
        TouchSimulation.Disable();
    }

    void Start()
    {
        m_initialZOffset = -transform.position.z;
    }

    public void OnMoveBall(Finger finger)
    {
        // Create a mathematical plane to map the ball position.
        Plane plane = new Plane(m_camera.transform.forward, m_initialZOffset);

        float distance;
        
        // Create a ray from the position of where the finger is on the screen.
        Ray ray = m_camera.ScreenPointToRay(finger.screenPosition);

        // Set a dummy world position value.
        Vector3 worldPosition = Vector3.zero;

        // If a plane-ray intersection did happen, get the world position of it.
        if(plane.Raycast(ray, out distance))
            worldPosition = ray.GetPoint(distance);
        
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
