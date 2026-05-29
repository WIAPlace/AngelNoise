using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class AbilityState_Aim : AbilityState_Abs
{
    [SerializeField] protected CinemachineCamera cinCam;

    [SerializeField,Tooltip("Added to base fov")] private float fovIn;
    [SerializeField] float zoomInTime;
    [SerializeField] float zoomOutTime;
    private float originalFOV;

    Coroutine zoomer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(brain == null) brain = GetComponent<Ability_Brain>();
        //cinCam = brain.cinCam;
        originalFOV = cinCam.Lens.FieldOfView;
        fovIn = originalFOV-fovIn; // make it based off of the base amount        
    }
    
    /////////////////////////////////// DO ENTER
    public override void DoEnter()
    {
        ResetTriggers();
        brain.anim.SetTrigger(AimHash);

        zoomer = StartCoroutine(ZoomIn(fovIn,zoomInTime));
        // When the state begins.
    }

    /////////////////////////////////// DO EXIT
    public override void DoExit()
    {
        if (zoomer != null)
        {
            StopCoroutine(zoomer);
        }
        zoomer = StartCoroutine(ZoomIn(originalFOV,zoomOutTime));

        



        // When the state is over.
    }

    /////////////////////////////////// DO STATE
    public override AbilityState_Abs DoState()
    {
        return this;
    }

    IEnumerator ZoomIn(float change, float zoomTime)
    {
        float startValue = cinCam.Lens.FieldOfView;
        
        float elapsed = 0f;
        
        while (elapsed < zoomTime)
        {
            // Calculate a percentage (0 to 1) of how much time has passed
            float percentage = elapsed / zoomTime;

            // Smoothly interpolate between start and target
            float current = Mathf.SmoothStep(startValue, change, percentage);
            
            cinCam.Lens.FieldOfView = current;

            elapsed += Time.deltaTime; // Track time since last frame
            yield return null; // Wait for next frame before continuing
        }
        cinCam.Lens.FieldOfView = change;
    }


    void OnDestroy()
    {
       if (zoomer != null)
        {
            StopCoroutine(zoomer);
        } 
    }
}
