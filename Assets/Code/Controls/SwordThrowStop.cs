using System;
using UnityEditor.Callbacks;
using UnityEngine;

public class SwordThrowStop : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask stoppingMask;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask boundsMask;

    [Header("Collision Detection")]
    [SerializeField] private float swordLength;
    [SerializeField] private float swordRadius;
    [SerializeField] private LayerMask hitMask;

    float maxDistance = 5f;
    

    private bool frozen = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject);
        //Debug.Log(collision.gameObject.layer);
        if(!frozen && (stoppingMask.value & (1 << collision.gameObject.layer)) != 0){
            //rb.constraints = RigidbodyConstraints.FreezeAll;
            //frozen = true;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if((playerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            GlobalEventManager.TriggerCaughtEvent();
            Destroy(gameObject);
        } 
    }

    // if it goes out of bounds
    void OnTriggerExit(Collider other)
    {
        if((boundsMask.value & (1 << other.gameObject.layer)) != 0)
        {
            GlobalEventManager.TriggerToobEvent();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Define the start point (tail) and end point (tip + direction)
        Vector3 startPos = transform.position;// - (brain.tempSword.transform.up * halfSword);
        Vector3 direction = transform.up;
        float distance = swordLength;

        // Perform the SphereCast
        if (Physics.SphereCast(startPos, swordRadius, direction, out RaycastHit hit, distance, hitMask))
        {
            rb.constraints = RigidbodyConstraints.FreezeAll; 
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 startPos = transform.position;// - (brain.tempSword.transform.up * halfSword);
        Vector3 direction = transform.up;
        float distance = swordLength;

        bool isHit = Physics.SphereCast(startPos, swordRadius, direction, out RaycastHit hit, distance, hitMask);

        // 2. Draw the starting sphere position
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(startPos, swordRadius);

        if (isHit)
        {
            // 3. Draw a green preview when something is hit
            Gizmos.color = Color.green;
            
            // Calculate center point where the sphere actually stopped
            Vector3 hitSphereCenter = startPos + direction * hit.distance;
            
            // Draw the sphere at its collision point
            Gizmos.DrawWireSphere(hitSphereCenter, swordRadius);
                
            // Draw lines connecting the outer bounds of the cast path
            DrawCastConnectors(startPos, hitSphereCenter, direction);
            // Optional: Draw a solid small dot exactly where the surface contact point is
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hit.point, 0.1f);
        }
        else
        {
            // 4. Draw a red preview when the cast misses entirely
            Gizmos.color = Color.red;
                
            // Calculate center point at max distance
            Vector3 endSphereCenter = startPos + direction * maxDistance;
                
            // Draw the final sphere at maximum range
            Gizmos.DrawWireSphere(endSphereCenter, swordRadius);
                
            // Draw lines connecting the outer bounds to maximum range
            DrawCastConnectors(startPos, endSphereCenter, direction);
        }
    }
    

    // Helper method to draw lines connecting the sides of the start and end spheres
    private void DrawCastConnectors(Vector3 start, Vector3 end, Vector3 dir)
    {
        // Find orthogonal vectors to find the "edges" of the sphere path
        Vector3 up = Vector3.Cross(dir, Vector3.right).normalized * swordRadius;
        if (up == Vector3.zero) up = Vector3.Cross(dir, Vector3.up).normalized * swordRadius;
        Vector3 right = Vector3.Cross(dir, up).normalized * swordRadius;

        // Draw 4 connecting lines spanning the capsule body
        Gizmos.DrawLine(start + up, end + up);
        Gizmos.DrawLine(start - up, end - up);
        Gizmos.DrawLine(start + right, end + right);
        Gizmos.DrawLine(start - right, end - right);
    }
}
