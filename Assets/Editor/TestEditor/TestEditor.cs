using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
//using Codice.Client.Common.FsNodeReaders;


// testing editor for bounding boxes. will be used for flying enemies and the like.
public class TestEditor : EditorWindow
{
    private ObjectField dataAssetField;
    //private ObjectField targetObjectField;
    //private BoundsField boundsField;
    private Button calculateButton; 
    private IntegerField waypointsField;

    private Button createWaypointsButton;
    private Button clearWaypointsButton;

    // The list UI control that lets you add/remove slots
    private ListView transformListView;
    private ListView boundsDisplayListView;
    // This list ONLY lives in the window's memory while open. It is never saved to an asset.
    private List<Transform> localTransformsList = new List<Transform>();

    ObjectField parentObjectField;
    //public GameObject prefabToInstantiate;

    private Transform rootTransform; // used for showing gizmos of children
    


    // The underlying data list holding the transforms
    //private List<Transform> targetTransformsList = new List<Transform>();
    //private List<Bounds> displayedBoundsList = new List<Bounds>();

    // The registry key used to save the asset path on your computer
    private const string SAVED_ASSET_PATH_KEY = "BoundsWindow_LastAssetPath";

    [MenuItem("Tools/TestEditor")]
    public static void ShowExample()
    {
        TestEditor wnd = GetWindow<TestEditor>();
        wnd.titleContent = new GUIContent("Test");
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += DrawBoundsInSceneView;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= DrawBoundsInSceneView;
    }

    public void CreateGUI()
    {
        dataAssetField = new ObjectField("Save Data Asset") { objectType = typeof(BoundsDataAsset) };

        // 1. CREATE DROPDOWN FOR TARGETS (Foldout)
        Foldout targetsFoldout = new Foldout();
        targetsFoldout.text = "Target Transforms List";
        targetsFoldout.style.unityFontStyleAndWeight = FontStyle.Bold;
        targetsFoldout.style.marginTop = 10;
        targetsFoldout.value = false; // Set to true so it defaults to open/expanded

        transformListView = new ListView();
        transformListView.itemsSource = localTransformsList;
        transformListView.showAddRemoveFooter = true;
        transformListView.reorderable = true;
        transformListView.style.maxHeight = 140;
        transformListView.makeItem = () => new ObjectField() { objectType = typeof(Transform) };
        transformListView.bindItem = (VisualElement element, int index) =>
        {
            ObjectField objField = element as ObjectField;
            objField.value = localTransformsList[index];
            objField.RegisterValueChangedCallback(evt =>
            {
                localTransformsList[index] = evt.newValue as Transform;
            });
        };

        transformListView.viewController.itemsSourceSizeChanged += () => OnTransformListSizeChanged();
        
        // Add the list view INSIDE the dropdown panel
        targetsFoldout.Add(transformListView);


        // 2. CREATE DROPDOWN FOR BOUNDS FIELDS (Foldout)
        Foldout boundsFoldout = new Foldout();
        boundsFoldout.text = "Calculated Individual Bounds";
        boundsFoldout.style.unityFontStyleAndWeight = FontStyle.Bold;
        boundsFoldout.style.marginTop = 15;
        boundsFoldout.value = true; // Defaults to open/expanded

        boundsDisplayListView = new ListView();
        boundsDisplayListView.style.maxHeight = 250;
        boundsDisplayListView.fixedItemHeight = 45; 

        boundsDisplayListView.makeItem = () => 
        {
            VisualElement rowContainer = new VisualElement();
            rowContainer.style.flexDirection = FlexDirection.Row;
            rowContainer.style.alignItems = Align.Center;

            Label indexLabel = new Label("Bounds X");
            indexLabel.style.width = 65; 
            indexLabel.style.unityFontStyleAndWeight = FontStyle.Bold;

            BoundsField boundsField = new BoundsField();
            boundsField.style.flexGrow = 1;

            rowContainer.Add(indexLabel);
            rowContainer.Add(boundsField);
            return rowContainer;
        };

        boundsDisplayListView.bindItem = (VisualElement element, int index) =>
        {
            BoundsDataAsset asset = dataAssetField.value as BoundsDataAsset;
            if (asset == null) return;

            Label indexLabel = element.Q<Label>();
            BoundsField boundsField = element.Q<BoundsField>();

            indexLabel.text = $"Bounds {index}";
            boundsField.SetValueWithoutNotify(asset.individualBoundsList[index]);
            boundsField.userData = index;

            boundsField.UnregisterValueChangedCallback(OnBoundsFieldValueChanged);
            boundsField.RegisterValueChangedCallback(OnBoundsFieldValueChanged);
        };

        // Add the bounds display list INSIDE the second dropdown panel
        boundsFoldout.Add(boundsDisplayListView);

        // Button for calculating new bounds based off of the transforms.
        calculateButton = new Button(CalculateAllBounds) { text = "Calculate All Bounds", style = { marginTop = 15 } };


        // Intiger Field
        waypointsField = new IntegerField("Number of Waypoints");
        // change the value inside of the SO when value is changed in editor
        waypointsField.RegisterValueChangedCallback(OnWaypointFieldChange); 


        // object field for waypoints to be instantiated on
        parentObjectField = new ObjectField("Parent Transform") 
        {
            objectType = typeof(Transform),
            allowSceneObjects = true,
            style = { marginTop = 15 }
        };
        parentObjectField.RegisterValueChangedCallback(evt =>
        {
           rootTransform = evt.newValue as Transform;
            SceneView.RepaintAll(); // update scene view impeiatly 
        });

        // create Waypoint button
        createWaypointsButton = new Button(CreateWaypoints) { text = "Create Waypoints", style = { marginTop = 0 } };
        createWaypointsButton.clicked += CreateWaypoints;   
        // Clear Waypoint button
        clearWaypointsButton = new Button(OnClearWaypoints) { text = "Clear Waypoints", style = { marginTop = 0 } };
        clearWaypointsButton.clicked += OnClearWaypoints;

        ///////////////////////////////////////// Button is only avalible if parent transform is set up.
        // 2. Define the enabling toggle logic
        void UpdateFieldState(Object newValue)
        {
            // Unity overrides the '==' operator, so evaluating against 'null' is safe
            bool hasValidObject = newValue != null; 
            createWaypointsButton.SetEnabled(hasValidObject);
            clearWaypointsButton.SetEnabled(hasValidObject);
        }
        UpdateFieldState(parentObjectField.value);

        parentObjectField.RegisterValueChangedCallback(evt => 
        {
            UpdateFieldState(evt.newValue);
        });


        ////////////////////////////////////////////////////////////////////////////////////////////////// Final Step
        // load data asset
        dataAssetField.RegisterValueChangedCallback(evt =>
        {
            LoadAssetData(evt.newValue as BoundsDataAsset);
        });

        // 3. BUILD LAYOUT (Adding the foldouts to the root window container)
        rootVisualElement.Add(dataAssetField);
        rootVisualElement.Add(targetsFoldout); // Adds the targets dropdown
        rootVisualElement.Add(calculateButton);
        rootVisualElement.Add(boundsFoldout);  // Adds the calculated bounds dropdown

        rootVisualElement.Add(parentObjectField);
        rootVisualElement.Add(waypointsField); // add waypoints intiger field
        rootVisualElement.Add(createWaypointsButton);
        rootVisualElement.Add(clearWaypointsButton);
        

        LoadLastUsedAsset();
    }

    // Shared value tracking event loop to avoid unregister/null errors completely
    private void OnBoundsFieldValueChanged(ChangeEvent<Bounds> evt)
    {
        BoundsDataAsset asset = dataAssetField.value as BoundsDataAsset;
        BoundsField boundsField = evt.target as BoundsField;
        
        if (asset != null && boundsField != null && boundsField.userData is int index)
        {
            if (index >= 0 && index < asset.individualBoundsList.Count)
            {
                asset.individualBoundsList[index] = evt.newValue;
                EditorUtility.SetDirty(asset);
                SceneView.RepaintAll();
            }
        }
    }

    private void LoadLastUsedAsset()
    {
        if (EditorPrefs.HasKey(SAVED_ASSET_PATH_KEY))
        {
            string lastPath = EditorPrefs.GetString(SAVED_ASSET_PATH_KEY);
            BoundsDataAsset loadedAsset = AssetDatabase.LoadAssetAtPath<BoundsDataAsset>(lastPath);
            if (loadedAsset != null)
            {
                dataAssetField.value = loadedAsset;
            }
        }
    }

    private void LoadAssetData(BoundsDataAsset asset)
    {
        localTransformsList.Clear();

        if (asset != null)
        {
            boundsDisplayListView.itemsSource = asset.individualBoundsList;
            waypointsField.value = asset.waypoints;

            for (int i = 0; i < asset.individualBoundsList.Count; i++)
            {
                localTransformsList.Add(null);
            }

            string assetPath = AssetDatabase.GetAssetPath(asset);
            EditorPrefs.SetString(SAVED_ASSET_PATH_KEY, assetPath);
        }
        else
        {
            boundsDisplayListView.itemsSource = null;
            EditorPrefs.DeleteKey(SAVED_ASSET_PATH_KEY);
        }

        transformListView.RefreshItems();
        boundsDisplayListView.RefreshItems();
        SceneView.RepaintAll();
    }

    private void OnTransformListSizeChanged()
    {
        BoundsDataAsset asset = dataAssetField.value as BoundsDataAsset;
        if (asset == null) return;

        while (asset.individualBoundsList.Count < localTransformsList.Count)
        {
            asset.individualBoundsList.Add(new Bounds());
        }
        while (asset.individualBoundsList.Count > localTransformsList.Count)
        {
            asset.individualBoundsList.RemoveAt(asset.individualBoundsList.Count - 1);
        }

        EditorUtility.SetDirty(asset);
        boundsDisplayListView.RefreshItems();
    }

    private void CalculateAllBounds()
    {
        BoundsDataAsset asset = dataAssetField.value as BoundsDataAsset;
        if (asset == null) return;

        for (int i = 0; i < localTransformsList.Count; i++)
        {
            Transform target = localTransformsList[i];
            if (target == null) continue;

            Bounds currentTargetBounds;
            Renderer[] renderers = target.GetComponentsInChildren<Renderer>();

            if (renderers.Length > 0)
            {
                currentTargetBounds = renderers[0].bounds;
                for (int j = 1; j < renderers.Length; j++)
                {
                    currentTargetBounds.Encapsulate(renderers[j].bounds);
                }
            }
            else
            {
                currentTargetBounds = new Bounds(target.position, target.lossyScale);
            }

            asset.individualBoundsList[i] = currentTargetBounds;
        }

        EditorUtility.SetDirty(asset);
        boundsDisplayListView.RefreshItems();
        SceneView.RepaintAll();
    }

    private void DrawBoundsInSceneView(SceneView sceneView)
    {
        if(dataAssetField!=null) {
            BoundsDataAsset asset = dataAssetField.value as BoundsDataAsset;
            if (asset == null) return;
        
            Handles.color = Color.cyan;
            foreach (Bounds indBounds in asset.individualBoundsList)
            {
                Handles.DrawWireCube(indBounds.center, indBounds.size);
            }
        }
        if(rootTransform != null)
        {
            Handles.color = Color.cyan;
            DrawGizmosRecursive(rootTransform);
        }
    }
    private void OnWaypointFieldChange(ChangeEvent<int> evt)
    {
        if(dataAssetField!=null) {
            BoundsDataAsset asset = dataAssetField.value as BoundsDataAsset;
            if (asset == null) return;

            asset.waypoints = evt.newValue;
        }
    }


    private void CreateWaypoints()
    {
        
        Transform selectedParent = parentObjectField.value as Transform;
        BoundsDataAsset asset = dataAssetField.value as BoundsDataAsset;

        if(selectedParent == null)
        { // shouldn't occure because button is disabled if transform is nothing
            return;
        }
        if(waypointsField.value <= 0)
        {
            
            return;
        }
        // for each bounds add waypoints at random positions
        for(int b = 0; b < asset.individualBoundsList.Count;b++){
            if(asset.individualBoundsList.Count<=0) return;

            for(int i=0;i<waypointsField.value;i++){
                //Debug.Log("created "+i);
                Vector3 randomPos = GetRandomPositionInBounds(b);
                GameObject waypoint = new GameObject($"Waypoint_{b}_{i}");

                waypoint.transform.position = randomPos;

                waypoint.transform.SetParent(selectedParent);
            }
        }
        SceneView.RepaintAll(); // update scene view impeiatly 
        //Undo.RegisterCreatedObjectUndo(selectedParent, "Spawned Object under Parent");
    }   

    private void OnClearWaypoints()
    {
        Transform parentTransform = parentObjectField.value as Transform;

        if (parentTransform == null)
        {
            Debug.LogWarning("Please assign a Parent Transform first.");
            return;
        }

        // Register undo so the user can press Ctrl+Z to reverse the deletion
        Undo.RegisterCompleteObjectUndo(parentTransform.gameObject, "Delete All Children");

        // Loop backwards to prevent indexing errors as children are removed
        for (int i = parentTransform.childCount - 1; i >= 0; i--)
        {
            Transform child = parentTransform.GetChild(i);
            DestroyImmediate(child.gameObject);
        }
        SceneView.RepaintAll(); // update scene view impeiatly 
    }

    // Get Random Position within a bounds
    private Vector3 GetRandomPositionInBounds(int bound)
    {
        BoundsDataAsset asset = dataAssetField.value as BoundsDataAsset;
        if(asset.individualBoundsList == null) return Vector3.zero;

        Bounds currentBound = asset.individualBoundsList[bound];

        float x = Random.Range(currentBound.min.x,currentBound.max.x);
        float y = Random.Range(currentBound.min.y,currentBound.max.y);
        float z = Random.Range(currentBound.min.z,currentBound.max.z);

        return new Vector3(x,y,z);
    }

    private void DrawGizmosRecursive(Transform parent)
    {
        // Draw a wireframe box or sphere at the current transform's position
        Handles.DrawWireCube(parent.position, Vector3.one * 0.5f);

        // Recursively draw for all children
        foreach (Transform child in parent)
        {
            //Handles.DrawLine(parent.position, child.position);
            DrawGizmosRecursive(child);
        }
    }
}