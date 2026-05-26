using UnityEngine;

public class AbilityState_Attack : AbilityState_Abs
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    /////////////////////////////////// DO ENTER
    public override void DoEnter()
    {
        // When the state begins.
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
}
