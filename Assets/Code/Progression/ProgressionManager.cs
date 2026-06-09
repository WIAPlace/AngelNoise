using System;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager Instance{get;private set;}
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

    [SerializeField]private GameObject player;
    [SerializeField] private GameObject[] UIEffects;

    public static LayerMask playerMask;

    public static event Action<int> KeyPickUpEvent; // 0 = red, 1 = blue, 2 = yellow 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        playerMask = 1 << gameObject.layer;
        foreach(GameObject ui in UIEffects)
        {
            ui.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KeyPickedUp(int index)
    {   // send of the index to the door and all that
        KeyPickUpEvent?.Invoke(index);

        if(index < UIEffects.Length && UIEffects[index]!=null){
            UIEffects[index].SetActive(true);
        }
    }
    public GameObject GetPlayer()
    {
        return player;
    }
}
