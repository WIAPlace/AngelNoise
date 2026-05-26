using System.Collections;
using UnityEngine;

public class AbilityState_Fire : AbilityState_Abs
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePosition;
    [SerializeField,Tooltip("Used to not have the projectile hit the player imediatly")] 
    private LayerMask projectileMask;
    
    [Header("Weapon variables")]
    [SerializeField,Tooltip("Time Inbetween Shots")] public float coolDown;
    [SerializeField] private float rechargeTime;
    [SerializeField] private int maxAmount;

    

    private CharacterController charCon;

    private Coroutine shooting;
    
    private float fireTime=.1f;
    private int currentAmount;

    void Start()
    {
        if(brain == null) brain = GetComponent<Ability_Brain>();
        charCon = brain.controls.controller;
    }

    /////////////////////////////////// DO ENTER
    public override void DoEnter()
    {
        
        shooting = StartCoroutine(Fire());
        
    }
    /////////////////////////////////// DO EXIT
    public override void DoExit()
    {
        brain.StopCo(shooting);
    }
    /////////////////////////////////// DO STATE
    public override AbilityState_Abs DoState()
    {
        return this;
    }

    IEnumerator Fire()
    {
        // make it so the projectile wont hit you when fired at a weird angle
        charCon.excludeLayers = projectileMask;

        // Spawn Projectile
        Instantiate(projectilePrefab,firePosition.position,firePosition.rotation); 
        yield return new WaitForSeconds(fireTime);

        // Go back to vulnerable to being hit
        charCon.excludeLayers = 0;

        brain.ChangeState(brain.idleState);
    }
}
