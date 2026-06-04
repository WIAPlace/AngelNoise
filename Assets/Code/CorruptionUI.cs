using UnityEngine;

public class CorruptionUI : MonoBehaviour
{
    public GameObject player;
    public GameObject redUI, blueUI, yellowUI;

    void Update()
    {
        if (player.GetComponent<PlayerInventory>().hasRed)
        {
            redUI.SetActive(true);
        }
        if (player.GetComponent<PlayerInventory>().hasBlue)
        {
            blueUI.SetActive(true);
        }
        if (player.GetComponent<PlayerInventory>().hasYellow)
        {
            yellowUI.SetActive(true);
        }
    }

}