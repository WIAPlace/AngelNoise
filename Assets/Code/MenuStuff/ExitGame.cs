using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// 
/// Author: Weston Tollette
/// Created: 2/9/26
/// Purpose: Used to exit the game
/// 
/// Edited:
/// Edited By:
/// Edit Purpose:
/// 
public class ExitGame : MonoBehaviour
{
    // function to exit the game
    public void DoExitGame()
    {
        Application.Quit();
        // testing to make sure it works in the editor.
        //Debug.Log("Game is exiting");  
    }
}
