using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AbilityState_Attack : AbilityState_Abs
{
    

    [SerializeField] private GameObject hitBox;
    [SerializeField] private Transform view;

    [SerializeField] EndOfAnim swingEnd;
    [SerializeField] LayerMask enemyMask;
    [SerializeField] private float hitRange;




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
        // reset triggers
        ResetTriggers();

        brain.anim.SetTrigger(SwingHash);
        HitZone();
        hitBox.SetActive(true);

        // When the state begins.
    }
    /////////////////////////////////// DO EXIT
    public override void DoExit()
    {
        brain.anim.ResetTrigger(SwingHash);
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

    private void HitZone()
    {
        Ray ray = new Ray(view.transform.position, view.transform.forward);
        RaycastHit hit;

        // Draw a debug ray in the Scene view to visualize it
        Debug.DrawRay(ray.origin, ray.direction * hitRange, Color.red, 1f);

        if (Physics.Raycast(ray, out hit, hitRange, enemyMask))
        {
            if(hit.collider.gameObject.TryGetComponent<IEntityHit>(out IEntityHit hitInterface)){
                hitInterface.Hit(view.position);
            }
        }
    }
}
