using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public static SpawnManager Instance{get;private set;}
    private void Awake()
    {
        // 2. Check if an instance already exists
        if (Instance != null && Instance != this)
        {
            // Destroy this duplicate component (or the entire GameObject)
            Destroy(gameObject); 
            return;
        }

        // 3. Assign the single instance
        Instance = this;
    }

    [SerializeField] private GameObject[] roomSpawns;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(GameObject room in roomSpawns)
        {
            if(room!=null)room.SetActive(false);
        }
    }

    public void SpawnRoom(int index)
    {
        if(index < roomSpawns.Length && !roomSpawns[index].activeSelf)
        {
            roomSpawns[index].SetActive(true);
        }  
    }
}
