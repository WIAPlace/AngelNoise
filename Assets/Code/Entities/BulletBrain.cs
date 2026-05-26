using System;
using System.Collections;
using UnityEngine;

public class BulletBrain : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private LayerMask levelMask;
 
    [SerializeField] private float bulletSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnEnable()
    {
        rb.linearVelocity = transform.forward * bulletSpeed;
        //StartCoroutine(GraceTime());
    }

    /*
    // use if we have problems with projectiles hitting the thing that made them
    IEnumerator GraceTime()
    {
        rb.excludeLayers = -1;
        yield return new WaitForSeconds(.05f);
        rb.excludeLayers = 0;
    }
    */

    void OnCollisionEnter(Collision collision)
    {
        
        if ((hitMask.value & (1 << collision.gameObject.layer)) != 0)
        { // check if it is of a layer that it can damage / effect

            //return;   
        }
        if ((levelMask.value & (1 << collision.gameObject.layer)) != 0)
        { //terminates self if hits the level geometry
            Destroy(gameObject); 
            return;
        }
    }

    
}
