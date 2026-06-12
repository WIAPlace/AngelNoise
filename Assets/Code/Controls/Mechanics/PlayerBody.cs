using Unity.VisualScripting;
using UnityEngine;

public class PlayerBody : MonoBehaviour, IPlayerHit
{
    [SerializeField] private int health;
    [SerializeField] private LayerMask projectileLayer;
    [SerializeField] private int projectileDamage;
    public void Hit(int damage)
    {
        Debug.Log("Hit For "+ damage+" Damage" );
        health -= damage;
        if(health <= 0)
        { // Death
            Debug.Log("Death");
        }
    }
    /*
    void OnTriggerEnter(Collider other)
    {
        if((projectileLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            Hit(projectileDamage);
        }
    }
    */
}
