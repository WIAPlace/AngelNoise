using UnityEngine;

public class SwordMoveWithRb : MonoBehaviour
{
    [SerializeField] Transform rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = rb.position;
        transform.rotation = rb.rotation;
    }
}
