using KinoGlitch;
using UnityEngine;

public class GlitchWhenNear : MonoBehaviour
{
    public AnalogGlitchController analogGlitch;
    public float intensity;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            Vector3 distanceVector = other.transform.position - transform.position;
            analogGlitch.ScanLineJitter = intensity / distanceVector.magnitude;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            analogGlitch.ScanLineJitter = 0;
        }
    }

    private void OnDestroy()
    {
        if(analogGlitch != null) analogGlitch.ScanLineJitter = 0;
    }
}
