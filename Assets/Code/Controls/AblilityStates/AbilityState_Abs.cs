using UnityEngine;

public abstract class AbilityState_Abs : MonoBehaviour
{
    protected Ability_Brain brain;
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
}
