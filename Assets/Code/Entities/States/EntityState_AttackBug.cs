using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EntityState_AttackBug : EntityState_Attack
{
    private Rigidbody rb;
    [SerializeField] protected float forwardForce = 10f;
    [SerializeField] private float upwardForce = 8f;
    /////////////////////////////////// DO ENTER
    public override void DoEnter()
    {
        //Debug.Log("attackstate Entered");
        
        rb=brain.rb;
        
        if (brain.agent.isActiveAndEnabled)
        {
            brain.agent.enabled = false;
        }
        if (rb.isKinematic)
        {
            rb.isKinematic = false;
        }
        StartCoroutine(LundgeAttack());
    }
    /////////////////////////////////// DO EXIT
    public override void DoExit()
    {   // When the state is over.
        if(brain.attacking == true)
        { // clean this up just in case
            brain.attacking = false;
        }
        if (!brain.agent.isActiveAndEnabled)
        {
            brain.agent.enabled = true;
        }
        if (!rb.isKinematic)
        {
            rb.isKinematic = true;
        }
    }
    /////////////////////////////////// DO STATE
    public override EntityState_Abs DoState()
    {
        return this;
    }

    IEnumerator LundgeAttack()
    {
        brain.attacking = true;
        yield return new WaitForSeconds(windUp);
        ResetAnims();
        if(brain.spriteAnim!=null)brain.spriteAnim.SetTrigger("Attack");
        attackSO.Play(brain.audioSource);
        
        LeapAtTarget();

        yield return new WaitForSeconds(windDown);
        brain.attacking = false;

        brain.ChangeState(brain.moveState);

    }

    private void LeapAtTarget()
    {
        if (target == null) return;
        //Debug.Log("Leaping");
        // 1. Get the flat direction toward the player (ignore Y height differences initially)
        Vector3 directionToPlayer = target.transform.position - transform.position;
        directionToPlayer.y = 0; // Ensures the enemy doesn't dive into the floor
        directionToPlayer.Normalize();

        // 2. Combine the horizontal rush with the vertical jump
        Vector3 leapVelocity = (directionToPlayer * forwardForce) + (Vector3.up * upwardForce);

        // 3. Reset existing velocity so the jump is consistent
        rb.linearVelocity = Vector3.zero; 

        // 4. Launch! Impulse mode is perfect for sudden bursts of movement
        rb.AddForce(leapVelocity, ForceMode.Impulse);
    }

    void OnTriggerEnter(Collider other)
    {
        if(brain.attacking && (playerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            brain.attacking = false;
            if(other.gameObject.TryGetComponent<IPlayerHit>(out IPlayerHit hitInterface)){
                hitInterface.Hit(attackDamage);
            }
            
        }
        else if(brain.attacking &&(hitMask.value & (1 << other.gameObject.layer)) != 0)
        {
            brain.attacking=false;
        }
    }
    
}
