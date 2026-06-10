using UnityEngine;

public class NewDoors : MonoBehaviour
{
    public Animator doorAnim;
    [SerializeField,Tooltip("Int relates to index of gameobject in spawn manager.\n 0 will spawn nothing.")]private int areaToSpawn;

    public bool requiresKey;
    public bool reqRed, reqBlue, reqYellow;

    private bool openAble;
    private bool[] keyAndGate;
    private LayerMask playerMask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (requiresKey)
        {
            ProgressionManager.KeyPickUpEvent +=OnKeyPickup;
            openAble = false;

            keyAndGate = new bool[] {reqRed,reqBlue,reqYellow};
        }
        else
        {
            openAble = true;
        }
        playerMask = ProgressionManager.playerMask;
    }
    void OnDestroy()
    {
        ProgressionManager.KeyPickUpEvent -= OnKeyPickup;
    }

    private void OnKeyPickup(int index)
    {
        // 0 = red, 1 = blue, 2 = yellow
        for(int i = 0; i < keyAndGate.Length; i++)
        {
            if (keyAndGate[i] && index != i)
            {
                return;
            }
        }
        openAble = true;

        // unsubscribe since we dont need this any more
        ProgressionManager.KeyPickUpEvent -= OnKeyPickup;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(openAble && (playerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            // doorAnim.SetTrigger("OpenDoor");
            doorAnim.SetBool("Open", true);
            // spawn shit
            if(areaToSpawn > 0) SpawnManager.Instance.SpawnRoom(areaToSpawn);
        }
    }
}
