using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management
/// 
/// Author: Weston Tollette
/// Created: 2/9/26
/// Purpose: Used to change the scene based off of the name inputed.
/// 
/// Edited:
/// Edited By:
/// Edit Purpose:
/// 
public class ChangeScene : MonoBehaviour
{
    public void LoadSceneByName(string sceneName)
    { // change scene to string name. Scene must be registered by the editor.
        //Debug.Log("Hit");
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1; // jsut to make sure the time is working as it should
    }
}
