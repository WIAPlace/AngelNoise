using UnityEngine;

public class PlayerBody : MonoBehaviour, IPlayerHit
{
    public void Hit(int damage)
    {
        Debug.Log("Hit For "+ damage+" Damage" );
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
