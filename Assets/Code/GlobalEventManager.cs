using System;
using UnityEngine;

// A script used for holding and activating events.
public static class GlobalEventManager
{
    // Events
    public static event Action CaughtEvent;
    public static event Action ThrownOutOfBoundsEvent;



    // Invokers of those events
    public static void TriggerCaughtEvent() // Caught event
    {
        CaughtEvent?.Invoke();
    }

    public static void TriggerToobEvent()
    {
        ThrownOutOfBoundsEvent?.Invoke();
    }


}
