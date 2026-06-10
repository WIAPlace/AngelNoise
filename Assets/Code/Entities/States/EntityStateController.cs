using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EntityStateController : MonoBehaviour, IEntityHit
{
    [field: SerializeField] public Rigidbody rb {get ;private set;}
    [field: SerializeField] public NavMeshAgent agent {get ;private set;}
    [SerializeField,Tooltip("How many hits they can take")] private int health = 3;

    // States
    [Header("States")]
    [field: SerializeField] public EntityState_Move moveState {get ;private set;}
    [field: SerializeField] public EntityState_Hit hitState {get ;private set;}
    [field: SerializeField] public EntityState_Attack AttackState {get ;private set;}



    public EntityState_Abs currentState {get ;private set;} = null;
    public EntityState_Abs previousState {get ;private set;} = null;

    // used for seeing what state we are in in the  inspector
    public string debugCurrentStateName;
    public string debugPreviousStateName;


    // Hit Back
    [HideInInspector] public LayerMask hitMask {get ;private set;}
    [HideInInspector] public float hitForce {get ;private set;}
    [HideInInspector] public float hitDuration {get ;private set;}

    private Vector3 dir; // holder for direction

    Coroutine runner;
    private bool hitAble= true;
    [HideInInspector] public bool attacking = false; 

    private float attackRange;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Putting these in seprate script just so this one doesnt get too busy.
        hitMask = hitState.hitMask;
        hitForce = hitState.hitForce;
        hitDuration = hitState.hitDuration;
        attackRange = AttackState.attackRange;
        attacking = false; // we do not start off attacking

        ChangeState(moveState);

    }

    // Update is called once per frame
    void Update()
    {
       if (currentState != null)
        {
            EntityState_Abs tempCheck = currentState.DoState();
            if(currentState != tempCheck) 
            { // using this as a of being able to utilize change state instead of just changing current state dirrectly
                ChangeState(tempCheck);
            }
            debugCurrentStateName = currentState.GetType().Name; //used for debuging to see name
            debugPreviousStateName = previousState?.GetType().Name; //used for debuging to see name
        } 
    }



    /////////////////////////////////////////////////////////////// Chanage State
    public void ChangeState(EntityState_Abs newState)
    {
        previousState = currentState;
        currentState?.DoExit(); // leave the prevvious state
        currentState = newState;
        currentState?.DoEnter(); // enter the new state   
    }


    //////////////////////////////////////////////////////////////////////////////////////// HIT Interface Stuff

    public void Hit(Vector3 direction)
    {// push entity in direction
        if(hitAble){ // stops from repededly being hit.
            hitAble = false; 
            direction -= transform.position;
            dir = -direction;
            //rb.excludeLayers = hitMask;
            health -= 1;
            if(health <= 0)
            {
                Destroy(gameObject);
                return;
            }
            ApplyPhysicsKnockback(dir,hitForce,hitDuration);
        }
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
        if(!attacking){
            ChangeState(hitState);
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
            ChangeState(moveState);
        }
        else
        {
            yield return new WaitForSeconds(duration); // just have some frames that they can't hit again
        }
        hitAble = true;
        
        //rb.excludeLayers = 0;
    }
}
