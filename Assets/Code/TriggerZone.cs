using UnityEngine;

public class TriggerZone : MonoBehaviour
{
  public GameObject thing;
    public bool turnOff;

    private void OnTriggerEnter(Collider other)
    {
        if (turnOff)
        {
            thing.SetActive(false);
        }
        else
        {
            thing.SetActive(true);
        }
    }
}
