using System.Collections;
using System.Data;
using NUnit.Framework.Constraints;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;

// Should only activate if last state was aim
public class AbilityState_Throw : AbilityState_Abs
{
    [Header("Refrences")]
    [SerializeField] private GameObject rbSword;
    [SerializeField] private Transform throwPoint;

    [SerializeField] private CinemachineImpulseSource impSour;
    

    [Header("Values")]
    [SerializeField] private float throwForce;
    [SerializeField] private float swordLength;
    [SerializeField] private float swordRadius;
    [SerializeField] private LayerMask hitMask;

    float maxDistance = 5f;
    

    

    private Transform view;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(brain == null) brain = GetComponent<Ability_Brain>();
        view = brain.cinCam.transform;
    }

    /////////////////////////////////// DO ENTER
    public override void DoEnter()
    {
        brain.sword.SetActive(false); // make it invisible and un usable

        /////////////////////////////////////////////// Thrown Weapon
        //Create object from prefab
        brain.tempSword = Instantiate(rbSword,throwPoint.position,throwPoint.rotation);
        brain.tempRb = brain.tempSword.GetComponent<Rigidbody>();
        
        // enable Gravity
        brain.tempRb.useGravity = true;

        // add force
        brain.tempRb.AddForce(throwPoint.forward *throwForce, ForceMode.VelocityChange);

        // add component for keeping rotation correct
        brain.stf = brain.tempSword.AddComponent<SwordThrowForward>();
        //////////////////////////////////////////////////////

        // game feel for throwing something
        impSour.GenerateImpulse();

        // change state to idle
        brain.ChangeState(brain.idleState);
        //StartCoroutine(DetectCollision());

    }
    /////////////////////////////////// DO EXIT
    public override void DoExit()
    {
        // When the state is over.
    }
    /////////////////////////////////// DO STATE
    public override AbilityState_Abs DoState()
    {
        return this;
    }

    IEnumerator DetectCollision()
    {   
        float halfSword =  swordLength/2;
        while (!brain.holdingSword)
        {           
            // Define the start point (tail) and end point (tip + direction)
            Vector3 startPos = brain.tempSword.transform.position;// - (brain.tempSword.transform.up * halfSword);
            Vector3 direction = brain.tempSword.transform.up;
            float distance = swordLength;

            // Perform the SphereCast
            if (Physics.SphereCast(startPos, swordRadius, direction, out RaycastHit hit, distance, hitMask))
            {
                // Check if we hit an obstacle first (like a wall)
                //Debug.Log("Hit");
                brain.tempRb.constraints = RigidbodyConstraints.FreezeAll; 
            }
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if(brain!=null && brain.tempSword != null){
            float halfSword =  swordLength/2;
            Vector3 startPos = brain.tempSword.transform.position;// - (brain.tempSword.transform.up * halfSword);
            Vector3 direction = brain.tempSword.transform.up;
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
