using UnityEngine;
using UnityEngine.Rendering.Universal;

// being plased here because i cant seem to find a way to turn this thing off in the scene view only
public class RenderSettings : MonoBehaviour
{
    [SerializeField] private bool inScene;
    [SerializeField] private ScriptableRendererFeature rendererFeature;
    [SerializeField] private bool activeState;

    private void OnEnable()
    {
        SetRenderFeatures();
        if(!inScene){
            inScene = true;
        }
    }
    private void OnValidate()
    {   
        if(inScene){
            SetRenderFeatures();
        }
        else if (activeState)
        {
            rendererFeature.SetActive(false);
        }
    }


    // will be used if multible are in existance
    private void SetRenderFeatures()
    {
        rendererFeature.SetActive(activeState);
    }
}
