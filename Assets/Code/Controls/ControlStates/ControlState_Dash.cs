using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.Cinemachine;
using UnityEngine;

public class ControlState_Dash : ControlState_Abs
{
    [Header("Movement")]
    [SerializeField, Tooltip("How far the dash will be")] private float dashDist;
    [SerializeField, Tooltip("How much time the dash will last")] private float dashTime;
    private Vector3 dashVelocity;
    [SerializeField, Tooltip("Cool down between dashes")] private float dashCoolDown;

    private Vector3 dashDir;
    private Vector2 dashDirV2; // used to see the vector 2 that was put in at time of press

    Coroutine dashCo;
    
    private Vector3 currentDashVelocity;
    private Vector3 dashSmoothingVelocity; // Required for SmoothDamp

    [Header("Camera Effect")]
    [SerializeField] private CinemachineCamera cam;
    [SerializeField, Tooltip("Dutch Strength")] private float dutch;
    [SerializeField, Tooltip("FOV Change")] private float fovChange;
    [SerializeField, Range(0,1),
    Tooltip("percentage of the way through the dash that the ease in will switch to ease out of the effect")] 
    private float easeSwitch;
    private float splitTime; // variable for when the switch will happen
    private float currentDutch; // used so we don't have snaps when switching 
    private float baseFOV;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseFOV = cam.Lens.FieldOfView;
    }

    /////////////////////////////////// DO ENTER
    public override void DoEnter()
    {
        dashDirV2 = brain.moveDir;
        splitTime = dashTime*easeSwitch;

        dashDir = playerBody.transform.forward * brain.moveDir.y + playerBody.transform.right * brain.moveDir.x;
        if(dashDir == Vector3.zero)
        {
            dashDir = -playerBody.transform.forward;
        }
        dashCo = StartCoroutine(Dash());
    }
    /////////////////////////////////// DO EXIT
    public override void DoExit()
    {
        currentDashVelocity = Vector3.zero; // set velocity to 0
        //brain.StopCo(dashCo);
    }
    /////////////////////////////////// DO STATE
    public override ControlState_Abs DoState()
    {
        return this;
    }

    private IEnumerator Dash() {
        float elapsed = 0f;

        Vector3 dashTarget = dashDir * (dashDist / dashTime);

        

        while (elapsed < dashTime)
        {
            // Smoothly ramp the velocity UP and DOWN using SmoothDamp
            currentDashVelocity = Vector3.SmoothDamp(currentDashVelocity, dashTarget, ref dashSmoothingVelocity, 0.05f);
            
            // move the character controller
            controller.Move(currentDashVelocity * Time.deltaTime);
            
            // visual effect

            // Left or right. Keeping this as a top level so it doesnt have to check all of them each time.
            if(dashDirV2.x != 0)
            {
                if(dashDirV2.x > 0) // right
                {
                    cam.Lens.Dutch = DutchEffectIn(elapsed,0, -dutch);
                }
                else // left
                {
                    cam.Lens.Dutch = DutchEffectIn(elapsed,0, dutch);
                }
            }
            // forward or back
            if(dashDirV2.y != 0)
            {
                if(dashDirV2.y > 0) // forward
                {
                    cam.Lens.FieldOfView = DutchEffectIn(elapsed, baseFOV, fovChange);
                }
                else // back
                {
                    cam.Lens.FieldOfView = DutchEffectIn(elapsed, baseFOV, -fovChange);
                }
            }

            elapsed += Time.deltaTime;

            // Optional: Start slowing down toward the end of the duration
            //if (elapsed > dashTime * 0.8f){ dashTarget = Vector3.zero;

            yield return null;
        }
        cam.Lens.Dutch = 0f;
        cam.Lens.FieldOfView = baseFOV;
        // start the timer for cool down.
        brain.dashTimer = StartCoroutine(brain.DashCoolDown(dashCoolDown));
    }

    private float DutchEffectIn(float elapsed, float start, float target)
    {        
        target += start;
        float t = elapsed / splitTime;
        float smoothT=0;

        if (t <= 1)
        {
            smoothT = Mathf.SmoothStep(start, target, t); 
            //currentDutch = smoothT;
            //Debug.Log(t);
        }
        else
        {
            // caluclate how deep into phase 2 we are.
            float phase2time = elapsed - splitTime;
            float phase2Dur = dashTime - splitTime; 

            float phase2Progress = phase2time/phase2Dur;


            smoothT = Mathf.SmoothStep(target, start, phase2Progress);
            //Debug.Log(phase2Progress); 
        }
        //Debug.Log(smoothT);
        
        return smoothT;
    }
}
