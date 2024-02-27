using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlineRenderPass : ScriptableRenderPass
{
    private readonly RTHandle renderHandle;
    private List<ShaderTagId> _shaderTagIdList = new() { new ShaderTagId("UniversalForward") };
    private FilteringSettings _filteringSettings;
    private RenderStateBlock _renderStateBlock;
    private Material _overrideMaterial;
    
    public OutlineRenderPass(RTHandle renderTargetHandle, int layerMask, Material overrideMaterial)
    {
        renderHandle = renderTargetHandle;
        _filteringSettings = new FilteringSettings(RenderQueueRange.opaque, layerMask);
        _overrideMaterial = overrideMaterial;
        _renderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);
    }
    
    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
        cmd.GetTemporaryRT(Shader.PropertyToID(renderHandle.name), cameraTextureDescriptor);
        ConfigureTarget(renderHandle);
        ConfigureClear(ClearFlag.All, Color.clear);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        SortingCriteria sortingCriteria = renderingData.cameraData.defaultOpaqueSortFlags;
        DrawingSettings drawingSettings = CreateDrawingSettings(_shaderTagIdList, ref renderingData, sortingCriteria);
        drawingSettings.overrideMaterial = _overrideMaterial;
        
        context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref _filteringSettings, ref _renderStateBlock);
    }
}