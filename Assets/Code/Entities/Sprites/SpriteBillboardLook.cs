using UnityEngine;

public class SpriteBillboardLook : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] private bool canLookVertically;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target!= null)
        {
            if(canLookVertically){
                transform.LookAt(target);
            }
            else
            {   // dont change verticle angle. 
                Vector3 modifiedTarget = target.position;
                modifiedTarget.y = transform.position.y;
                transform.LookAt(modifiedTarget);
            }
        }
    }
}
