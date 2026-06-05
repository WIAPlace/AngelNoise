using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


public class WaypointGeneratorWindow : EditorWindow
{
    private Transform parentTransform;
    private int numberOfWaypoints = 10;

    private int sectors; // how many boxes there are
    private Vector3[] boxCenter = new Vector3[5];// = Vector3.zero;
    private Vector3[] boxSize = new Vector3[5];//(10f, 10f, 10f);


    private bool showDebugBox = true;
    private Color debugBoxColor = Color.yellow;


    private bool showWaypointGizmos = true;
    private float waypointGizmoSize = 0.5f;
    private Color waypointGizmoColor = Color.cyan;


    private List<GameObject> generatedWaypoints = new List<GameObject>();




    [MenuItem("Tools/Random Waypoint Generator")]
    public static void ShowWindow()
    {
        GetWindow<WaypointGeneratorWindow>("Random Waypoint Generator");
    }


    private void OnGUI()
    {
        parentTransform = (Transform)EditorGUILayout.ObjectField("Parent Transform", parentTransform, typeof(Transform), true);


        numberOfWaypoints = EditorGUILayout.IntField("Number of Waypoints", numberOfWaypoints);
        sectors = EditorGUILayout.IntField("Number of Sectors", sectors);


        EditorGUILayout.Space();


        EditorGUILayout.LabelField("Box Settings", EditorStyles.boldLabel);
        for(int i = 0; i<sectors;i++){
            EditorGUILayout.LabelField("Box ["+i+"]", EditorStyles.boldLabel);
            boxCenter[i] = EditorGUILayout.Vector3Field("Box ["+i+"] Center (World)", boxCenter[i]);
            boxSize[i] = EditorGUILayout.Vector3Field("Box ["+i+"] Size", boxSize[i]);
            EditorGUILayout.Space();
        }


        EditorGUILayout.Space();


        EditorGUILayout.LabelField("Debug Visualization", EditorStyles.boldLabel);
        showDebugBox = EditorGUILayout.Toggle("Show Box in Scene", showDebugBox);
        debugBoxColor = EditorGUILayout.ColorField("Box Color", debugBoxColor);


        showWaypointGizmos = EditorGUILayout.Toggle("Show Waypoints in Scene", showWaypointGizmos);
        waypointGizmoColor = EditorGUILayout.ColorField("Waypoints Color", waypointGizmoColor);
        waypointGizmoSize = EditorGUILayout.Slider("Waypoint Gizmo Size", waypointGizmoSize, 0.1f, 2f);


        EditorGUILayout.Space();


        if (GUILayout.Button("Generate Random Waypoints"))
        {
            GenerateRandomWaypoints();
        }


        if (GUILayout.Button("Clear Generated Waypoints"))
        {
            ClearGeneratedWaypoints();
        }
    }




    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }




    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }


    private Vector3 GetRandomPositionInBox(int box)
    {
        Vector3 halfSize = boxSize[box] * 0.5f;


        float x = Random.Range(boxCenter[box].x - halfSize.x, boxCenter[box].x + halfSize.x);
        float y = Random.Range(boxCenter[box].y - halfSize.y, boxCenter[box].y + halfSize.y);
        float z = Random.Range(boxCenter[box].z - halfSize.z, boxCenter[box].z + halfSize.z);


        return new Vector3(x, y, z);
    }




    private void GenerateRandomWaypoints()
    {
        if (numberOfWaypoints <= 0)
        {
            return;
        }

        for(int b = 0; b<sectors;b++){
            for (int i = 0; i < numberOfWaypoints; i++)
            {
                Vector3 randomPos = GetRandomPositionInBox(b);


                GameObject waypoint = new GameObject($"Waypoint_{i}");
                waypoint.transform.position = randomPos;


                if (parentTransform != null)
                {
                    waypoint.transform.SetParent(parentTransform);
                }


                generatedWaypoints.Add(waypoint);
            }
        }
    }




    private void ClearGeneratedWaypoints()
    {
        for (int i = generatedWaypoints.Count - 1; i >= 0; i--)
        {
            if (generatedWaypoints[i] != null)
            {


                Undo.DestroyObjectImmediate(generatedWaypoints[i]);
            }
        }


        generatedWaypoints.Clear();
    }




    private void OnSceneGUI(SceneView sceneView)
    {
        if (showDebugBox)
        {
            Handles.color = debugBoxColor;
            for(int i = 0; i<sectors;i++){
                Handles.DrawWireCube(boxCenter[i], boxSize[i]);
            }
        }


        if (showWaypointGizmos && generatedWaypoints != null)
        {
            Handles.color = waypointGizmoColor;
            foreach (var wp in generatedWaypoints)
            {
                if (wp == null) continue;
                Handles.DrawWireDisc(wp.transform.position, Vector3.up, waypointGizmoSize);
            }
        }
    }
}

