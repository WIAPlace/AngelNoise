using Unity.VisualScripting;
using UnityEngine;

public class PlayerBody : MonoBehaviour, IPlayerHit
{
    [SerializeField] private int health;
    public void Hit(int damage)
    {
        Debug.Log("Hit For "+ damage+" Damage" );
        health -= damage;
        if(health <= 0)
        { // Death
            Debug.Log("Death");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
