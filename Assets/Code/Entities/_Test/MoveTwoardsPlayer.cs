using UnityEngine;
using UnityEngine.AI;

public class MoveTwoardsPlayer : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject target; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(agent.isOnNavMesh && target != null){
            agent.SetDestination(target.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.isOnNavMesh && target != null){
            agent.SetDestination(target.transform.position);
        }
    }
}
