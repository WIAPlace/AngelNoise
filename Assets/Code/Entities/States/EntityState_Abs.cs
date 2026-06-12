using System;
using UnityEngine;

public abstract class EntityState_Abs : MonoBehaviour
{
    protected EntityStateController brain;

    private void OnEnable()
    {
        StartUp();
    }


    /////////////////////////////////// DO ENTER
    public virtual void DoEnter()
    {   // When the state begins.
        
    }
    /////////////////////////////////// DO EXIT
    public virtual void DoExit()
    {   // When the state is over.
        
    }
    /////////////////////////////////// DO STATE
    public virtual EntityState_Abs DoState()
    {
        return this;
    }

    protected void StartUp()
    {
        brain = GetComponent<EntityStateController>();
    }

    protected void ResetAnims()
    {
        if (brain.spriteAnim != null)
        {
            brain.spriteAnim.ResetTrigger("Walk");
            brain.spriteAnim.ResetTrigger("Attack");
        }
    }

}
