using UnityEngine;

public class AbilityState_Idle : AbilityState_Abs
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(brain == null) brain = GetComponent<Ability_Brain>();
    }

    /////////////////////////////////// DO ENTER
    public override void DoEnter()
    {
        if(brain.previousState != null && brain.previousState == brain.aimState){
            brain.anim.SetTrigger(UnAimHash);
        }
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
