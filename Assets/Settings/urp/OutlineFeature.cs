using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlineFeature : ScriptableRendererFeature
{
    [SerializeField] private string _renderTextureName;
    [SerializeField] private RenderSettings _renderSettings;
    [SerializeField] private string _bluredTextureName;
    [SerializeField] private BlurSettings _blurSettings;
    [SerializeField] private Material _outlineMaterial;
    [SerializeField] private RenderPassEvent _renderPassEvent;
    private OutlinePass _outlinePass;
    private RTHandle _bluredTexture;
    private BlurPass _blurPass;
    private RTHandle _renderTexture;
    private ObjectsPass _renderPass;
    
    public override void Create()
    {
        _renderTexture = RTHandles.Alloc("ObjectsProperty", name: "ObjectsProperty");
        _bluredTexture = RTHandles.Alloc("BlurProperty", name: "BlurProperty");
        
        _renderPass = new ObjectsPass(_renderTexture, _renderSettings.LayerMask, _renderSettings.OverrideMaterial);
        _blurPass = new BlurPass(_blurSettings.BlurMaterial, _blurSettings.DownSample, _blurSettings.PassesCount);
        _outlinePass = new OutlinePass(_outlineMaterial);
        
        _renderPass.renderPassEvent = _renderPassEvent;
        _blurPass.renderPassEvent = _renderPassEvent;
        _outlinePass.renderPassEvent = _renderPassEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_renderPass);
        renderer.EnqueuePass(_blurPass);
        renderer.EnqueuePass(_outlinePass);
    }
    
    [Serializable]
    public class RenderSettings
    {
        public Material OverrideMaterial = null;
        public LayerMask LayerMask = 0;
    }
    
    [Serializable]
    public class BlurSettings
    {
        public Material BlurMaterial;
        public int DownSample = 1;
        public int PassesCount = 1;
    }
}
