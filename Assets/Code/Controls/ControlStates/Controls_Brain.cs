using System;
using System.Collections;
using UnityEngine;

// the brain of the player controller.
public class Controls_Brain : MonoBehaviour
{
    [field: SerializeField]
    public InputReader input {get;private set;} // putting this as public so i dont have to plug it in to every control state.
    public CharacterController controller;


    [field:SerializeField, Tooltip("Player Body")]
    public GameObject playerBody{get;private set;} 
    [SerializeField] [Tooltip("First Person Camera attached to this so the body moves with the camera")]
    private Transform playerView;

    private ControlState_Abs currentState;
    private ControlState_Abs previousState;

    // used for seeing what state we are in in the  inspector
    public string debugCurrentStateName;
    public string debugPreviousStateName;

    // States
    [SerializeField] ControlState_Movement moveState;
    [SerializeField] ControlState_Dash dashState;

    // Physics shit
    private float gravity = -9.81f; // gravity
    private Vector3 velocity; // speed of gravity

    //[HideInInspector] 
    public Vector2 moveDir; // movment direction as seen via inputs
    
    private bool dashAble = true; // checks if the player is able to dash. Cool Down
    public Coroutine dashTimer;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input.MoveEvent += HandleMove;
        input.DashEvent += HandleDash;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        
        ChangeState(moveState);
    }
    void OnDestroy()
    {
        input.MoveEvent -= HandleMove; // unsubscribe
        input.DashEvent -= HandleDash;
    }

    // Update is called once per frame
    void Update()
    {
        RotateToView(); // rotate with camera 
        if (currentState != null)
        {
            ControlState_Abs tempCheck = currentState.DoState();
            if(currentState != tempCheck) 
            { // using this as a of being able to utilize change state instead of just changing current state dirrectly
                ChangeState(tempCheck);
            }
            debugCurrentStateName = currentState.GetType().Name; //used for debuging to see name
            debugPreviousStateName = previousState?.GetType().Name; //used for debuging to see name
        }
        Gravity(); // be affected by gravity
    }

    /////////////////////////////////////////////////////////////// Chanage State
    public void ChangeState(ControlState_Abs newState)
    {
        previousState = currentState;
        currentState?.DoExit(); // leave the prevvious state
        currentState = newState;
        currentState?.DoEnter(); // enter the new state   
    }


    ///////////////////////////////////////////////////////////////// Rotate to view
    private void RotateToView()
    { // the object / player turns to where the camera is looking
        playerBody.transform.rotation = Quaternion.Euler(
            playerBody.transform.eulerAngles.x, 
            playerView.eulerAngles.y, 
            playerBody.transform.eulerAngles.z);
        
    }
    ///////////////////////////////////////////////////////////////// Gravity
    private void Gravity()
    {
        // Reset velocity when on the ground
        if (controller.isGrounded && velocity.y < 0) {
            velocity.y = -2f; // Small downward force to keep grounded
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        // Move the controller
        controller.Move(velocity * Time.deltaTime);
    }

    public void StopCo(Coroutine co)
    { // will only stop if it is running.
        if(co != null)
        {
            StopCoroutine(co);
        }
    }

    // Dash Cool Down
    public IEnumerator DashCoolDown(float time)
    {
        ChangeState(moveState);
        yield return new WaitForSeconds(time);
        dashAble = true;
    }

    private void HandleMove(Vector2 moveAxis) // Move
    {
        moveDir = moveAxis;
    }
    private void HandleDash() // Dash
    {
        if (dashAble)
        {
            ChangeState(dashState);
            dashAble = false;
        }
    }
    
    
}
