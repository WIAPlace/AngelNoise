using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AbilityState_Attack : AbilityState_Abs
{
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject hitBox;
    [SerializeField] private Animator anim;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotSpeed;

    [SerializeField] EndOfAnim swingEnd;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(brain == null) brain = GetComponent<Ability_Brain>();
        EndOfAnim.SwingEnd += HandleAnimEnd;
    }

    void OnDestroy()
    {
        EndOfAnim.SwingEnd -= HandleAnimEnd;
    }

    /////////////////////////////////// DO ENTER
    public override void DoEnter()
    {
        //Debug.Log("Attack");
        //sword.transform.localPosition = basePosition.localPosition;
        //sword.transform.localRotation = basePosition.localRotation;
        anim.ResetTrigger("Block");
        anim.ResetTrigger("UnBlock");
        anim.ResetTrigger("Swing");


        anim.SetTrigger("Swing");
        hitBox.SetActive(true);

        // When the state begins.
    }
    /////////////////////////////////// DO EXIT
    public override void DoExit()
    {
        anim.ResetTrigger("Swing");
        hitBox.SetActive(false);
        // When the state is over.
    }
    /////////////////////////////////// DO STATE
    public override AbilityState_Abs DoState()
    {
        return this;
    }

    public void HandleAnimEnd()
    {
        brain.ChangeState(brain.idleState);
    }
}
