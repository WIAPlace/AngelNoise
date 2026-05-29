using UnityEngine;

// Sets up basicly a sheild.
public class AbilityState_Block : AbilityState_Abs
{
    [SerializeField] private GameObject blockObject; // basicly a sheild
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(brain == null) brain = GetComponent<Ability_Brain>();
        blockObject.SetActive(false);
    }

    /////////////////////////////////// DO ENTER
    public override void DoEnter()
    {// When the state begins.
        ResetTriggers(); // reset anim triggers
        brain.anim.SetTrigger(BlockHash);

        blockObject.SetActive(true);
        
    }
    /////////////////////////////////// DO EXIT
    public override void DoExit()
    {// When the state is over.
        
        blockObject.SetActive(false);

        ResetTriggers(); // reset anim triggers
        brain.anim.SetTrigger(UnBlockHash);
    }
    /////////////////////////////////// DO STATE
    public override AbilityState_Abs DoState()
    {
        return this;
    }

    // Could set up a corutine or transitional state to act as parry if wanted
}
