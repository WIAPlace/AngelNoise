using UnityEngine;

public class EntityState_Hit : EntityState_Abs
{
    [field: SerializeField] public LayerMask hitMask {get ;private set;}
    [field: SerializeField] public float hitForce {get ;private set;}
    [field: SerializeField] public float hitDuration {get ;private set;}

    /////////////////////////////////// DO ENTER
    public override void DoEnter()
    {   // When the state begins.
    }
    /////////////////////////////////// DO EXIT
    public override void DoExit()
    {   // When the state is over.
        
    }
    /////////////////////////////////// DO STATE
    public override EntityState_Abs DoState()
    {
        return this;
    }
}
