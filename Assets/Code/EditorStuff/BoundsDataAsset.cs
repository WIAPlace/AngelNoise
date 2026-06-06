using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewMultiBoundsData", menuName = "Tools/Multi-Bounds Data Asset")]
public class BoundsDataAsset : ScriptableObject
{
    // Saves each individual object's box separately in a list
    public List<Bounds> individualBoundsList = new List<Bounds>();
    public int waypoints;
}
