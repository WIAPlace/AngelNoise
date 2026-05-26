using System;
using UnityEngine;

public class EndOfAnim : MonoBehaviour
{
    [SerializeField] private Animator anim;
    
    //[SerializeField] private string eventName;

    public static event Action SwingEnd;

    public void EndOfAnimTrigger()
    {   // activate event if it is done
        //Debug.Log("Invoked");
        SwingEnd?.Invoke();
    }
}
