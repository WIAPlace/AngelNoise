using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator doorAnim;
    public GameObject areaToSpawn;

    public bool requiresKey;
    public bool reqRed, reqBlue, reqYellow;

    // opening door
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (requiresKey)
            {
                // more checks
                if(reqRed && other.GetComponent<PlayerInventory>().hasRed)
                {
                    // doorAnim.SetTrigger("OpenDoor");
                    doorAnim.SetBool("Open", true);
                    // spawn shit
                    areaToSpawn.SetActive(true);
                }
                if (reqBlue && other.GetComponent<PlayerInventory>().hasBlue)
                {
                    //doorAnim.SetTrigger("OpenDoor");
                    doorAnim.SetBool("Open", true);
                    // spawn shit
                    areaToSpawn.SetActive(true);
                }
                if (reqYellow && other.GetComponent<PlayerInventory>().hasYellow)
                {
                    //doorAnim.SetTrigger("OpenDoor");
                    doorAnim.SetBool("Open", true);
                    // spawn shit
                    areaToSpawn.SetActive(true);
                }
            }
            else
            {
                //doorAnim.SetTrigger("OpenDoor");
                doorAnim.SetBool("Open", true);
                // spawn shit
                areaToSpawn.SetActive(true);
            }
        }
    }

    
}
