using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EntityState_Attack : EntityState_Abs
{
    [field: SerializeField] public float attackRange;
    [SerializeField] protected int attackDamage;

    [SerializeField] protected float windUp;
    [SerializeField] protected float windDown;

    [SerializeField] protected LayerMask hitMask;
    [SerializeField] protected LayerMask playerMask;


    [Header("Bone Guy Only")]
    [SerializeField] private Sprite attackSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AngleToPlayer angleToPlayer;
    protected GameObject target;

    void Start()
    {
        target = ProgressionManager.Instance.GetPlayer();
    }
    
    /////////////////////////////////// DO ENTER
    public override void DoEnter()
    {
        angleToPlayer.walking = false;

        StartCoroutine(Attack());
    }
    /////////////////////////////////// DO EXIT
    public override void DoExit()
    {   // When the state is over.
        angleToPlayer.walking = true;
        if(brain.attacking == true)
        { // clean this up just in case
            brain.attacking = false;
        }
    }
    /////////////////////////////////// DO STATE
    public override EntityState_Abs DoState()
    {
        return this;
    }

    IEnumerator Attack()
    {
        brain.attacking = true;
        yield return new WaitForSeconds(windUp);
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        spriteRenderer.sprite = attackSprite;

        // Draw a debug ray in the Scene view to visualize it
        Debug.DrawRay(ray.origin, ray.direction * attackRange, Color.red, 1f);

        if (Physics.Raycast(ray, out hit, attackRange, hitMask)) 
        { // hit mask so its stoped by shield
            if((playerMask.value & (1 << hit.collider.gameObject.layer)) != 0 && 
                hit.collider.gameObject.TryGetComponent<IPlayerHit>(out IPlayerHit hitInterface)){
                hitInterface.Hit(attackDamage);
            }
        }
        brain.attacking = false;
        yield return new WaitForSeconds(windDown);
        brain.ChangeState(brain.moveState);
    }
}
