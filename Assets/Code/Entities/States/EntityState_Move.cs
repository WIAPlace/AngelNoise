using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class EntityState_Move : EntityState_Abs
{
    private NavMeshAgent agent;
    [SerializeField] private GameObject target;
    [Range(0f, 1f)] public float visionThreshold = 0.7f; // 0.7 is roughly a 45-degree angle cone (90 degrees total)
    float rotationSpeed;
    /////////////////////////////////// DO ENTER
    public override void DoEnter()
    {   // When the state begins.
        agent = brain.agent;
        
        rotationSpeed = agent.angularSpeed;
    }
    /////////////////////////////////// DO EXIT
    public override void DoExit()
    {   // When the state is over.
        agent.updateRotation = true;
    }
    /////////////////////////////////// DO STATE
    public override EntityState_Abs DoState()
    {
        if(agent.isOnNavMesh && target != null){
            agent.SetDestination(target.transform.position);

            float distanceToTarget = agent.remainingDistance;
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
            directionToTarget.y = 0;

            if(distanceToTarget != Mathf.Infinity && distanceToTarget <= agent.stoppingDistance)
            {   // if in swinging range
                if (agent.updateRotation)
                {
                    agent.updateRotation = false;
                }
                // 2. Calculate the dot product between our forward vector and the direction vector
                float dotProduct = Vector3.Dot(transform.forward, directionToTarget);

                // 3. Check if the dot product exceeds our threshold
                if (dotProduct >= visionThreshold)
                {
                    Debug.Log("attacking");
                    return brain.AttackState;
                }
            }
            else if(distanceToTarget != Mathf.Infinity)
            {  // if it is not turning on its own
                if (!agent.updateRotation)
                {
                    agent.updateRotation = true;
                }
            }

            if(!agent.updateRotation && directionToTarget != Vector3.zero){ // upate rotation even if close
                // rotate to target
                // Create a rotation looking at the target
                Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
                // Smoothly rotate towards the target
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }
        }
        return this;
    }


}
