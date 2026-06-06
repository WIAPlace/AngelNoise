using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;


// testing editor for bounding boxes. will be used for flying enemies and the like.
public class TestEditor : EditorWindow
{
    private ObjectField dataAssetField;
    private ObjectField targetObjectField;
    private BoundsField boundsField;
    private Button calculateButton; 

    // The list UI control that lets you add/remove slots
    private ListView transformListView;
    private ListView boundsDisplayListView;
    // This list ONLY lives in the window's memory while open. It is never saved to an asset.
    private List<Transform> localTransformsList = new List<Transform>();


    // The underlying data list holding the transforms
    private List<Transform> targetTransformsList = new List<Transform>();
    private List<Bounds> displayedBoundsList = new List<Bounds>();

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
        targetsFoldout.value = true; // Set to true so it defaults to open/expanded

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


        calculateButton = new Button(CalculateAllBounds) { text = "Calculate All Bounds", style = { marginTop = 15 } };

        dataAssetField.RegisterValueChangedCallback(evt =>
        {
            LoadAssetData(evt.newValue as BoundsDataAsset);
        });

        // 3. BUILD LAYOUT (Adding the foldouts to the root window container)
        rootVisualElement.Add(dataAssetField);
        rootVisualElement.Add(targetsFoldout); // Adds the targets dropdown
        rootVisualElement.Add(calculateButton);
        rootVisualElement.Add(boundsFoldout);  // Adds the calculated bounds dropdown
        

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
    }
}