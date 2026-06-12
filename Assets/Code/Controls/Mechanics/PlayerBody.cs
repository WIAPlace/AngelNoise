using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement; // Required namespace
using UnityEngine;

public class PlayerBody : MonoBehaviour, IPlayerHit
{
    [SerializeField] private int health;
    [SerializeField] private LayerMask projectileLayer;
    [SerializeField] private int projectileDamage;

    [Header("Health to ColorBanding")]
    [SerializeField] private FilterTool_ColorBanding banding;
    [SerializeField] private int startingValue;
    [SerializeField] private int minValue;

    [Header("Regen")]
    [SerializeField] private int regenTime;
    [SerializeField] private int RegenStopTime;

    private bool justHit;
    private void Start()
    {
        justHit = false;
        StartCoroutine(Regenerate());
    }

    public bool CheckActive()
    {
        return enabled;
    }

    public void Hit(int damage)
    {
        Debug.Log("Hit For "+ damage+" Damage" );
        health -= damage;

        
        
        if(damage>0) justHit = true;
        banding.ChangColorStep(damage);
        
        if(health <= 0)
        { // Death
            Debug.Log("Death");
            health = 0;
            ResetScene();
            // reset scene
        }
    }

    private IEnumerator Regenerate()
    {
        while (true)
        {
            if(!justHit){
                if(health < startingValue){
                    Hit(-1);
                }
                yield return new WaitForSeconds(regenTime);
            }
            else
            {
                justHit = false;
                yield return new WaitForSeconds(RegenStopTime);
            }
        }
    }

    public void ResetScene()
    {
        // Reloads the active scene using its index number
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
