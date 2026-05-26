using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;

public class TestShoot : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform cameraPos;
    [SerializeField] private InputReader input;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input.AttackEvent += HandleAttack;
    }
    void OnDestroy()
    {
        input.AttackEvent -= HandleAttack;
    }

    private void HandleAttack()
    {
        Instantiate(projectilePrefab,cameraPos.position,cameraPos.rotation);
    }
}
