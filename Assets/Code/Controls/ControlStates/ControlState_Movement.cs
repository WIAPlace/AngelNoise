using UnityEngine;

// Child of ControlState_abs. This is the controller for movement;
public class ControlState_Movement : ControlState_Abs
{
    [SerializeField] private float moveSpeed;
    //[SerializeField] private GameObject playerBody;
    public Vector2 moveDir;
    //[SerializeField] private CharacterController controller;

    /////////////////////////////////// DO ENTER
    public override void DoEnter() // When the state begins.
    {
        input.MoveEvent += HandleMove;
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
            return;
        }
        //Debug.Log("Moving");
        Vector3 directionalMovement = playerBody.transform.forward * moveDir.y + playerBody.transform.right * moveDir.x;

        // move in the desired direction.
        Vector3 movement = directionalMovement * moveSpeed * Time.deltaTime;
        controller.Move(movement);

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
