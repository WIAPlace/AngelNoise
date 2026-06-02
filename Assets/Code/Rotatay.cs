using UnityEngine;

public class Rotatay : MonoBehaviour
{
    public GameObject thing;
    public float xAngle;
    public float yAngle;
    public float zAngle;

    void Update()
    {
        thing.transform.Rotate(xAngle, yAngle, zAngle);
    }
}
