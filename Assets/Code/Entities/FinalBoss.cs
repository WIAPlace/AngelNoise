using UnityEngine;

public class FinalBoss : MonoBehaviour, IEntityHit
{
    public void Hit(Vector3 dir) // turns off the game or the editor
    {
        Debug.Log("Quit function called.");

        #if UNITY_EDITOR
            // Stops play mode if running inside the Unity Editor
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // Closes the application if running a standalone build
            Application.Quit();
        #endif
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
