using UnityEngine;

// use this to force the rigidbodies forward direction to be in line with the velocity direction
public class SwordThrowForward : MonoBehaviour
{
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        // Check that the spear is actually moving
        if (rb.linearVelocity.sqrMagnitude > 0.1f)
        {
            // 1. Get the direction the spear is traveling
            Vector3 travelDirection = rb.linearVelocity.normalized;

            // 2. Rotate the spear so its UP axis points toward the direction of travel
            // This forces the green axis to lead the flight path
            transform.rotation = Quaternion.FromToRotation(Vector3.up, travelDirection);

            // for if the forward is the top, probably should be for bullet stuff
            // transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }
    
}
