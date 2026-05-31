using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EntityBody : MonoBehaviour, IEntityHit
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private float hitForce;
    [SerializeField] private float hitDuration;

    private Vector3 dir; // holder for direction

    Coroutine runner;
    private bool hitAble= true;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hitAble && (hitMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            hitAble = false;
            // get the point of contact and convert it to the direction
            Hit(collision.transform.position - transform.position);
        }
    }

    public void Hit(Vector3 direction)
    {// push entity in direction
        dir = -direction;
        rb.excludeLayers = hitMask;
        ApplyPhysicsKnockback(dir,hitForce,hitDuration);
    }

    public void ApplyPhysicsKnockback(Vector3 direction, float force, float duration)
    {
        if(runner != null)
        {
            StopCoroutine(runner);
        }
        runner = StartCoroutine(PhysicsKnockbackRoutine(direction, force, duration));
    }

    private IEnumerator PhysicsKnockbackRoutine(Vector3 direction, float force, float duration)
    {
        // 1. Disengage the NavMesh control
        agent.enabled = false;
        
        // 2. Hand control over to the physics engine
        rb.isKinematic = false;
        
        // Clear Y to avoid launching characters into space unexpectedly
        direction.y = 0; 
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);

        // 3. Wait for the knockback timer to expire
        yield return new WaitForSeconds(duration);

        // 4. Reset physics constraints and bring velocity back to zero
        rb.linearVelocity = Vector3.zero;
        yield return new WaitForEndOfFrame();
        rb.isKinematic = true;
        

        // 5. Secure agent position on the path and reactivate pathfinding
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }
        
        agent.enabled = true;
        hitAble = true;
        rb.excludeLayers = 0;
    }
}
