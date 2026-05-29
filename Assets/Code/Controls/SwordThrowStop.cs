using System;
using JetBrains.Rider.Unity.Editor;
using UnityEditor.Callbacks;
using UnityEngine;

public class SwordThrowStop : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask stoppingMask;
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
        if((stoppingMask.value & (1 << collision.gameObject.layer)) != 0){
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
