using System;
using UnityEditor.Callbacks;
using UnityEngine;

public class SwordThrowStop : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask stoppingMask;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask boundsMask;

    private bool frozen = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject);
        //Debug.Log(collision.gameObject.layer);
        if(!frozen && (stoppingMask.value & (1 << collision.gameObject.layer)) != 0){
            rb.constraints = RigidbodyConstraints.FreezeAll;
            frozen = true;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if((playerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            GlobalEventManager.TriggerCaughtEvent();
            Destroy(gameObject);
        } 
    }

    // if it goes out of bounds
    void OnTriggerExit(Collider other)
    {
        if((boundsMask.value & (1 << other.gameObject.layer)) != 0)
        {
            GlobalEventManager.TriggerToobEvent();
        }
    }
}
