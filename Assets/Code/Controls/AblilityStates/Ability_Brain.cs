using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

// controller of the players abilities
public class Ability_Brain : MonoBehaviour
{
    [field: SerializeField] public Controls_Brain controls;

    [HideInInspector]
    public GameObject playerBody{get;private set;} 

    private Transform playerView;
    private InputReader input;

    private AbilityState_Abs currentState;
    private AbilityState_Abs previousState;

    // used for seeing what state we are in in the  inspector
    public string debugCurrentStateName;
    public string debugPreviousStateName;

    // States
    [field:SerializeField] public AbilityState_Attack attackState {get;private set;}
    [field:SerializeField] public AbilityState_Block blockState {get;private set;}
    [field:SerializeField] public AbilityState_Fire fireState {get;private set;}
    [field:SerializeField] public AbilityState_Idle idleState {get;private set;}


    [HideInInspector] public bool fireCoolDown = true; // cooldown for shots
    
    [HideInInspector] public bool swingCoolDown = true; // cooldown for attacks

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //set up
        input = controls.input;
        playerView = controls.playerView;
        playerBody = controls.playerBody;

        input.AttackEvent += HandleAttack;
        input.BlockEvent += HandleBlock;
        input.BlockEventCancelled += HandleBlockCancelled;
        input.FireEvent += HandleFire;

        ChangeState(idleState);

        fireCoolDown = true;
        swingCoolDown = true;
    }
    void OnDestroy()
    {
        input.AttackEvent -= HandleAttack;
        input.BlockEvent -= HandleBlock;
        input.BlockEventCancelled -= HandleBlockCancelled;
        input.FireEvent -= HandleFire;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != null)
        {
            AbilityState_Abs tempCheck = currentState.DoState();
            if(currentState != tempCheck) 
            { // using this as a of being able to utilize change state instead of just changing current state dirrectly
                ChangeState(tempCheck);
            }
            debugCurrentStateName = currentState.GetType().Name; //used for debuging to see name
            debugPreviousStateName = previousState?.GetType().Name; //used for debuging to see name
        }
    }

    /////////////////////////////////////////////////////////////// Chanage State
    public void ChangeState(AbilityState_Abs newState)
    {
        previousState = currentState;
        currentState?.DoExit(); // leave the prevvious state
        currentState = newState;
        currentState?.DoEnter(); // enter the new state   
    }

    public void StopCo(Coroutine co) ///////////////////////////////// Stop Corutines
    { // will only stop if it is running.
        if(co != null)
        {
            StopCoroutine(co);
            //co = null;
        }
    }
    public IEnumerator UniversalCoolDownMethod(System.Action<bool> coolBool, float coolTime)
    {
        coolBool(false);
        yield return new WaitForSeconds(coolTime);
        coolBool(true);
    }



    // Handlers
    private void HandleAttack() // attack
    {
        ChangeState(attackState);
    }
    private void HandleBlock() // block
    {
        ChangeState(blockState);
    }
    private void HandleBlockCancelled() // block cancelled
    {
        ChangeState(idleState);
    }
    private void HandleFire() // fire
    {
        //Debug.Log(fireCoolDown);
        if(fireCoolDown && !controls.dashing){ // you cant fire while dashing. 
            ChangeState(fireState);
            StartCoroutine(UniversalCoolDownMethod(val => fireCoolDown = val,fireState.coolDown));
        }
    }
    
}
