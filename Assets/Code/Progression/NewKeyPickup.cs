using UnityEngine;

public class NewKeyPickup : MonoBehaviour
{
    [SerializeField,Tooltip("0 = Red\n1 = Blue\n2 = Yellow")] private int keyCardIndex;

    private LayerMask playerMask;
    
    void Start()
    {
        playerMask = ProgressionManager.playerMask;
    }

    private void OnTriggerEnter(Collider other) // pick up the key related to the index
    {
        if((playerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            ProgressionManager.Instance.KeyPickedUp(keyCardIndex);
            Destroy(gameObject);
        }
    }
}
