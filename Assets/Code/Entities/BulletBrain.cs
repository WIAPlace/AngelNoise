using System;
using System.Collections;
using UnityEngine;

public class BulletBrain : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private LayerMask levelMask;
    [SerializeField] private LayerMask weaponMask;
    [SerializeField] private LayerMask worldMask;
 
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
        if ((weaponMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            Debug.Log("Weapon");
            transform.forward = collision.transform.forward;
            rb.linearVelocity = Vector3.zero;
            rb.linearVelocity = transform.forward * bulletSpeed*2;
            return;
        }
        //else
        if ((hitMask.value & (1 << collision.gameObject.layer)) != 0)
        { // check if it is of a layer that it can damage / effect
            
            //return;   
        }
        if ((levelMask.value & (1 << collision.gameObject.layer)) != 0)
        { //terminates self if hits the level geometry
            Debug.Log("Destroy");
            Destroy(gameObject); 
            return;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ((levelMask.value & (1 << other.gameObject.layer)) != 0)
        { 
            Destroy(gameObject);
        }

    }


}
