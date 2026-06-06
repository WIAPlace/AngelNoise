using UnityEngine;
using System.Collections.Generic;


public class WaypointHolder : MonoBehaviour
{   // when making this from the ground up next time it would be better to store Vector3s instead of empty transforms. 
    // Due to memory overhead, though since this is a small game it should be fine for the most part.
    public Transform[][] Waypoints;
    private int waypointsPerBound = 100;
    int currentBound = 0;
   
    private void OnValidate()
    {
        RefreshWaypoints();
    }
    public void RefreshWaypoints()
    {
        Waypoints = new Transform[(transform.childCount/waypointsPerBound)+1][];
        currentBound = 0;
        int bound = 0;
        List<Transform> wpList = new List<Transform>();

        for (int i = 0; i < transform.childCount; i++)
        {
            bound = i/waypointsPerBound;
            if(currentBound != bound)
            {
                //Debug.Log(i);
                //Debug.Log(bound);

                Waypoints[currentBound] = wpList.ToArray();
                wpList.Clear();
                currentBound = bound;
            }

            Transform child = transform.GetChild(i);

            wpList.Add(child);

            
            if (i >= transform.childCount - 1)
            {   // final group catch.
                //Debug.Log(i);
                //Debug.Log(bound);

                Waypoints[bound] = wpList.ToArray();
            }
        }
    }
}

