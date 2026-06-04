using UnityEngine;
using UnityEngine.InputSystem;

public class SpriteRotator : MonoBehaviour
{
    private Transform target;

    void Start()
    {
        target = FindFirstObjectByType<CharacterController>().transform;
        
    }

    void Update()
    {
        transform.LookAt(target);
        transform.Rotate(0, 180, 0);
    }
}
