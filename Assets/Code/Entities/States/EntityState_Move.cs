using UnityEngine;
using UnityEngine.AI;

public class EntityState_Move : EntityState_Abs
{
    private NavMeshAgent agent;
    [SerializeField] private GameObject target; 
    /////////////////////////////////// DO ENTER
    public override void DoEnter()
    {   // When the state begins.
        agent = brain.agent;
    }
    /////////////////////////////////// DO EXIT
    public override void DoExit()
    {   // When the state is over.
        
    }
    /////////////////////////////////// DO STATE
    public override EntityState_Abs DoState()
    {
        if(agent.isOnNavMesh && target != null){
            agent.SetDestination(target.transform.position);
        }
        return this;
    }


}
