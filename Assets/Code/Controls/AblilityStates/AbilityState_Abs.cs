using UnityEngine;

public abstract class AbilityState_Abs : MonoBehaviour
{
    
    protected Ability_Brain brain;

    protected static readonly int SwingHash = Animator.StringToHash("Swing");
    protected static readonly int BlockHash = Animator.StringToHash("Block");
    protected static readonly int UnBlockHash = Animator.StringToHash("UnBlock");
    protected static readonly int AimHash = Animator.StringToHash("Aim");
    protected static readonly int UnAimHash = Animator.StringToHash("UnAim");

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        brain = GetComponent<Ability_Brain>();
    }

    /////////////////////////////////// DO ENTER
    public virtual void DoEnter()
    {
        
        // When the state begins.
    }
    /////////////////////////////////// DO EXIT
    public virtual void DoExit()
    {
        // When the state is over.
    }
    /////////////////////////////////// DO STATE
    public virtual AbilityState_Abs DoState()
    {
        return this;
    }

    protected void ResetTriggers()
    {
        brain.anim.ResetTrigger(BlockHash);
        brain.anim.ResetTrigger(UnBlockHash);
        brain.anim.ResetTrigger(SwingHash);
        brain.anim.ResetTrigger(AimHash);
        brain.anim.ResetTrigger(UnAimHash);
    }
}
