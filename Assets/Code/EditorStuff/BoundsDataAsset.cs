using UnityEngine;

// SO for saving data for bounds and other stuff for testing out
[CreateAssetMenu(fileName = "SavedBoundsData", menuName = "Tools/Bounds Data File")]
public class BoundsDataAsset : ScriptableObject
{
    public Bounds savedBounds;
}
