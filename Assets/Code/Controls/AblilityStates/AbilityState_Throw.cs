using System.Collections;
using System.Data;
using NUnit.Framework.Constraints;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;

// Should only activate if last state was aim
public class AbilityState_Throw : AbilityState_Abs
{
    [Header("Refrences")]
    [SerializeField] private GameObject rbSword;
    [SerializeField] private Transform throwPoint;

    [SerializeField] private CinemachineImpulseSource impSour;
    

    [Header("Values")]
    [SerializeField] private float throwForce;

    private Transform view;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(brain == null) brain = GetComponent<Ability_Brain>();
        view = brain.cinCam.transform;
    }

    /////////////////////////////////// DO ENTER
    public override void DoEnter()
    {
        brain.sword.SetActive(false); // make it invisible and un usable

        /////////////////////////////////////////////// Thrown Weapon
        //Create object from prefab
        brain.tempSword = Instantiate(rbSword,throwPoint.position,throwPoint.rotation);
        brain.tempRb = brain.tempSword.GetComponent<Rigidbody>();
        
        // enable Gravity
        brain.tempRb.useGravity = true;

        // add force
        brain.tempRb.AddForce(throwPoint.forward *throwForce, ForceMode.VelocityChange);

        // add component for keeping rotation correct
        brain.stf = brain.tempSword.AddComponent<SwordThrowForward>();
        //////////////////////////////////////////////////////

        // game feel for throwing something
        impSour.GenerateImpulse();

        // change state to idle
        brain.ChangeState(brain.idleState);
        //StartCoroutine(DetectCollision());

    }
    /////////////////////////////////// DO EXIT
    public override void DoExit()
    {
        // When the state is over.
    }
    /////////////////////////////////// DO STATE
    public override AbilityState_Abs DoState()
    {
        return this;
    }
}
