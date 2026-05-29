using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

// controller of the players abilities
public class Ability_Brain : MonoBehaviour
{
    
    [field: SerializeField] public Controls_Brain controls {get;private set;}
    [field: SerializeField] public CinemachineCamera cinCam {get;private set;}

    [HideInInspector]
    public GameObject playerBody {get;private set;} 

    private Transform playerView;
    private InputReader input;

    public AbilityState_Abs currentState {get ;private set;} = null;
    public AbilityState_Abs previousState {get ;private set;} = null;

    // used for seeing what state we are in in the  inspector
    public string debugCurrentStateName;
    public string debugPreviousStateName;

    // States
    [field:SerializeField] public AbilityState_Attack attackState {get;private set;}
    [field:SerializeField] public AbilityState_Block blockState {get;private set;}
    [field:SerializeField] public AbilityState_Aim aimState {get;private set;}
    [field:SerializeField] public AbilityState_Idle idleState {get;private set;}
    [field:SerializeField] public AbilityState_Throw throwState {get;private set;}
    [field:SerializeField] public AbilityState_PullBack pullState {get;private set;}

    [field:SerializeField] public GameObject sword {get;private set;}
    [field:SerializeField] public Animator anim {get;private set;}



    //[HideInInspector] public bool fireCoolDown = true; // cooldown for shots
    
    [HideInInspector] public bool swingCoolDown = true; // cooldown for attacks
    [HideInInspector] public GameObject tempSword;
    //[HideInInspector] 
    public Rigidbody tempRb;
    [HideInInspector] public SwordThrowForward stf; // used as a refrenced to be destroyed and entered;


    [HideInInspector] public bool holdingSword = true; // whether or not player is holding the sword at any current moment

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Debug.Log(cinCam);
        //Debug.Log(cinCam.Lens.FieldOfView);
        //set up
        input = controls.input;
        playerView = controls.playerView;
        playerBody = controls.playerBody;

        input.AttackEvent += HandleAttack;
        input.BlockEvent += HandleBlock;
        input.BlockEventCancelled += HandleBlockCancelled;
        input.FireEvent += HandleAim;
        input.FireEventCancelled += HandleAimCancelled;

        GlobalEventManager.CaughtEvent += HandleCaughtEvent;
        GlobalEventManager.ThrownOutOfBoundsEvent += HandleToobEvent;

        ChangeState(idleState);

        
        swingCoolDown = true;
        holdingSword = true;
    }
    void OnDestroy()
    {
        input.AttackEvent -= HandleAttack;
        input.BlockEvent -= HandleBlock;
        input.BlockEventCancelled -= HandleBlockCancelled;
        input.FireEvent -= HandleAim;
        input.FireEventCancelled -= HandleAimCancelled;

        GlobalEventManager.CaughtEvent -= HandleCaughtEvent;
        GlobalEventManager.ThrownOutOfBoundsEvent -= HandleToobEvent;

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

    /*
    public IEnumerator UniversalCoolDownMethod(System.Action<bool> coolBool, float coolTime)
    {
        coolBool(false);
        yield return new WaitForSeconds(coolTime);
        coolBool(true);
    }
    */


    // Handlers
    private void HandleAttack() // attack
    {
        if(holdingSword){
            if(currentState == aimState) // throw sword if aiming
            {
                //rbSword.isKinematic = false;
                holdingSword = false;
                ChangeState(throwState);
                return;
            }
            if(currentState != attackState){ // swing sword
                ChangeState(attackState);
            }
        }
        else
        {
            if(tempSword != null)
            {
                ChangeState(pullState);
            }
        }
    }
    private void HandleBlock() // block
    {
        ChangeState(blockState);
    }
    private void HandleBlockCancelled() // block cancelled
    {
        ChangeState(idleState);
    }
    private void HandleAim() // Aim
    {
        //Debug.Log(fireCoolDown);
        if(holdingSword && !controls.dashing && currentState != attackState){ // you cant fire while dashing. 
            ChangeState(aimState);
            //StartCoroutine(UniversalCoolDownMethod(val => fireCoolDown = val,fireState.coolDown));
        }
    }
    private void HandleAimCancelled() // aim cancelled
    {
        ChangeState(idleState);
        //else bring back the sword
    }
    
    private void HandleCaughtEvent()
    {
        sword.SetActive(true);
        holdingSword = true;
    }
    private void HandleToobEvent()
    {
        // Stop all forces on the object
        tempRb.constraints = RigidbodyConstraints.FreezeAll;
        // Force player to pull the sword back in
        ChangeState(pullState); 
    }
}
