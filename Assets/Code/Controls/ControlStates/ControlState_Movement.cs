//using System.Numerics;
using UnityEngine;

// Child of ControlState_abs. This is the controller for movement;
public class ControlState_Movement : ControlState_Abs
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float smoothMovement;

    public Vector2 moveDir;
    private Vector3 directionalMovement;
    private Vector3 targetVelocity;
    
    //private float velocity;

    private Vector3 currentVelocity;
    private Vector3 smoothDampVelocity;
    

    /////////////////////////////////// DO ENTER
    public override void DoEnter() // When the state begins.
    {
        input.MoveEvent += HandleMove;
        currentVelocity = Vector3.zero;
        smoothDampVelocity = Vector3.zero;
    }

    /////////////////////////////////// DO EXIT
    public override void DoExit() // When the state is over.
    {
        input.MoveEvent -= HandleMove; // unsubscribe
    }

    ////////////////////////////////// DO STATE
    public override ControlState_Abs DoState() // while the state is active
    {
        Move();
        return this;
    }

    private void HandleMove(Vector2 moveAxis)
    {
        moveDir = moveAxis;
    }

    private void Move()
    {
        if(moveDir == Vector2.zero)
        { // If the player has no controls down, so isnt touching any directional input, dont move
            //currentStepTime=stepTime; // reset both of these because we have stoped walking
            //newStepTime = 0;

            if(currentVelocity.magnitude > .1) // slow down till stopped
            {     
                // move to stop in previous direction.
                currentVelocity = Vector3.SmoothDamp(currentVelocity, Vector3.zero, ref smoothDampVelocity, smoothMovement);        
                controller.Move(currentVelocity *Time.deltaTime);  
            }
            else if(currentVelocity.magnitude != 0)
            {
                currentVelocity = Vector3.zero; // stop moving
            }

            return;
        }
        //Debug.Log("Moving");
        directionalMovement = playerBody.transform.forward * moveDir.y + playerBody.transform.right * moveDir.x;
        targetVelocity = directionalMovement * moveSpeed;
        
        // smooth damp for a bit of slide.
        currentVelocity = Vector3.SmoothDamp(currentVelocity, targetVelocity, ref smoothDampVelocity, smoothMovement);

        
        controller.Move(currentVelocity *Time.deltaTime);

        /*
        // audio for footsteps
        currentStepTime += Time.deltaTime;
        // loop walking sounds
        if (currentStepTime >= stepTime) //currentStepTime >= newStepTime &&
        {   // if the current step time is greater than the audio clip length and time between steps
            currentStepTime = 0;
            
            audioClipPlaying =  Random.Range(0, walking[terrainIndex].clips.Length); // audio clip that be playing
            //newStepTime = walking[terrainIndex].clips[audioClipPlaying].length;

            walking[terrainIndex].Play(footStepSource,audioClipPlaying);
        }
        */
    }
}
