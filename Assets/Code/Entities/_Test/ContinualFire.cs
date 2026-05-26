using System.Collections;
using UnityEngine;

public class ContinualFire : MonoBehaviour
{
    [SerializeField] Transform firePosition;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float inbetweenTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ConstantFire());
    }

    IEnumerator ConstantFire()
    {
        while (true)
        {
            Instantiate(projectilePrefab,firePosition.position,firePosition.rotation);
            yield return new WaitForSeconds(inbetweenTime);
        }
    }

    
}
