using System.Collections;
using UnityEngine;

// When u pull the sword back to the player.
public class AbilityState_PullBack : AbilityState_Abs
{
    [SerializeField] private float pullInSpeed;
    [SerializeField] private float rotSpeed;

    private Transform inHandPosition;
    private Quaternion startRot;
    private Rigidbody rb;
    
    private Coroutine pulling;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(brain == null) brain = GetComponent<Ability_Brain>();
        inHandPosition = brain.sword.transform;
        rb = brain.tempRb;
    }

    /////////////////////////////////// DO ENTER
    public override void DoEnter()
    {
        rb = brain.tempRb;
        startRot = rb.rotation;

        Destroy(brain.stf);

        ResetTriggers();
        brain.anim.SetTrigger(UnAimHash);
        
        rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = false;
        //rb.constraints = RigidbodyConstraints.FreezeRotation;

        // When the state begins.
        pulling = StartCoroutine(PullIn());
    }
    /////////////////////////////////// DO EXIT
    public override void DoExit()
    {
        // When the state is over.
    }
    /////////////////////////////////// DO STATE
    public override AbilityState_Abs DoState()
    {
        return brain.idleState; // only need to do what is needed in this state.
    }

    IEnumerator PullIn()
    {
        while (!brain.holdingSword) // while the sword is not in hand.
        {
            yield return new WaitForFixedUpdate();
            Vector3 newPosition = Vector3.MoveTowards(rb.position, inHandPosition.position, pullInSpeed * Time.fixedDeltaTime);
            Quaternion newRotation = Quaternion.RotateTowards(rb.rotation, inHandPosition.rotation, rotSpeed * Time.fixedDeltaTime);

            rb.MovePosition(newPosition);
            rb.MoveRotation(newRotation);
        }

        if(brain.currentState != null && brain.currentState == this)
        {
            brain.ChangeState(brain.idleState);
        }
    }
}
