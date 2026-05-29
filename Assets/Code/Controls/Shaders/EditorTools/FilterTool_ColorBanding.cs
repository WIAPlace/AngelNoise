using System;
using UnityEngine;

public class FilterTool_ColorBanding : FilterTool_Abs
{
    private static readonly int ColorStepID = Shader.PropertyToID("_ColorSteps");
    [SerializeField] private int colorSteps;

    public override void StartUpActivity()
    {
        rendererFeature.SetActive(activeState);
        fullScreenMat.SetFloat(ColorStepID,colorSteps);
    }

    
}
