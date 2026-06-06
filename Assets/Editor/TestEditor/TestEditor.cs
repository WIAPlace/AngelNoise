using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class TestEditor : EditorWindow
{
    private ObjectField dataAssetField;
    private ObjectField targetObjectField;
    private BoundsField boundsField;
    private Button calculateButton; 

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
        targetObjectField = new ObjectField("Target Transform") { objectType = typeof(Transform) };
        dataAssetField = new ObjectField("Save Data Asset") { objectType = typeof(BoundsDataAsset) };
        boundsField = new BoundsField("Calculated Bounds");
        calculateButton = new Button(CalculateAndDisplayBounds) { text = "Calculate Bounds" };

        // 1. When the user picks a different asset, save its path to EditorPrefs
        dataAssetField.RegisterValueChangedCallback(evt =>
        {
            BoundsDataAsset asset = evt.newValue as BoundsDataAsset;
            if (asset != null)
            {
                boundsField.value = asset.savedBounds;
                
                // Get the file path (e.g., "Assets/Data/MyBounds.asset") and save it
                string assetPath = AssetDatabase.GetAssetPath(asset);
                EditorPrefs.SetString(SAVED_ASSET_PATH_KEY, assetPath);
            }
            else
            {
                boundsField.value = new Bounds();
                EditorPrefs.DeleteKey(SAVED_ASSET_PATH_KEY); // Clear it if empty
            }
            SceneView.RepaintAll();
        });

        boundsField.RegisterValueChangedCallback(evt =>
        {
            SaveToAsset(evt.newValue);
            SceneView.RepaintAll();
        });

        rootVisualElement.Add(dataAssetField);
        rootVisualElement.Add(targetObjectField);
        rootVisualElement.Add(boundsField);
        rootVisualElement.Add(calculateButton);

        // 2. Load the last used asset automatically when the window opens
        LoadLastUsedAsset();
    }

    private void LoadLastUsedAsset()
    {
        if (EditorPrefs.HasKey(SAVED_ASSET_PATH_KEY))
        {
            string lastPath = EditorPrefs.GetString(SAVED_ASSET_PATH_KEY);
            
            // Try to find the asset file at that path
            BoundsDataAsset loadedAsset = AssetDatabase.LoadAssetAtPath<BoundsDataAsset>(lastPath);
            
            if (loadedAsset != null)
            {
                dataAssetField.value = loadedAsset; // This triggers the callback to update the UI bounds
            }
        }
    }

    private void CalculateAndDisplayBounds()
    {
        Transform target = targetObjectField.value as Transform;

        if (target == null) // dont change if there is nothing in there
        {
            //boundsField.value = new Bounds();
            //SaveToAsset(boundsField.value);
            //SceneView.RepaintAll();
            return;
        }

        Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            Bounds combinedBounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                combinedBounds.Encapsulate(renderers[i].bounds);
            }
            boundsField.value = combinedBounds;
        }
        else
        {
            boundsField.value = new Bounds(target.position, target.lossyScale);
        }

        SaveToAsset(boundsField.value);
        SceneView.RepaintAll(); 
    }

    private void SaveToAsset(Bounds boundsToSave)
    {
        BoundsDataAsset asset = dataAssetField.value as BoundsDataAsset;
        if (asset != null)
        {
            asset.savedBounds = boundsToSave;
            EditorUtility.SetDirty(asset);
        }
    }

    private void DrawBoundsInSceneView(SceneView sceneView)
    {
        BoundsDataAsset asset = dataAssetField.value as BoundsDataAsset;
        if (asset == null) return;

        Handles.color = Color.cyan;
        Handles.DrawWireCube(asset.savedBounds.center, asset.savedBounds.size);
    }
}