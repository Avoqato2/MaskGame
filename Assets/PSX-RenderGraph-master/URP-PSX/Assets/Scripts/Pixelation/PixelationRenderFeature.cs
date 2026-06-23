using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

namespace PSX
{
    public class PixelationRenderFeature : ScriptableRendererFeature
    {
        //[SerializeField] private Shader shader;
        [SerializeField] private Shader shader;
        [SerializeField] private RenderPassEvent injectionPoint = RenderPassEvent.BeforeRenderingPostProcessing;

        private Material _material;
        private PixelationRenderPass _pixelationRenderPass;
    
        public override void Create()
        {
            // If no shader is found, attempt to fetch it, else abort
            if (!shader)
            {
                shader = Shader.Find("Shader Graphs/Pixelation");
                if (!shader)
                {
                    Debug.LogError($"{nameof(PixelationRenderFeature)} skipped: Requires a shader to function.");
                    return;
                }
            }
        
            // Creates the material from shader
            _material = new Material(shader);
        
            // Creates the render pass
            _pixelationRenderPass = new PixelationRenderPass(_material)
            {
                renderPassEvent = injectionPoint
            };
        }


        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
        
#if UNITY_EDITOR
            // If post-processing is turned off in the scene view, don't pixelize
            if (renderingData.cameraData.isSceneViewCamera)
            {
                var sceneView = UnityEditor.SceneView.currentDrawingSceneView;
                if (sceneView != null && !sceneView.sceneViewState.showImageEffects)
                {
                    return;
                }
            }
#endif
        
            // Gets around an error where these are null switching scenes (this may be a bad solution)
            if (_pixelationRenderPass == null || _material == null)
            {
                Create();
            }

            // Enqueue the pass if both of these exist resources exist
            if (_pixelationRenderPass != null && _material != null)
            {
                renderer.EnqueuePass(_pixelationRenderPass);
            }
        }
    
        private class PixelationRenderPass : ScriptableRenderPass
        {
            private static readonly int s_PixelationWidth = Shader.PropertyToID("_PixelationWidth");
            private static readonly int s_PixelationHeight = Shader.PropertyToID("_PixelationHeight");
            private static readonly int s_ColorPrecision = Shader.PropertyToID("_ColorPrecision");

            private const string k_psxPassName = "Pixelation (PSX)";

            private readonly Material _material;
        
            private TextureDesc _pixelationTextureDesc;

            public PixelationRenderPass(Material material)
            {
                this._material = material;
            }

            private void RefreshMaterialValues(Pixelation pixelation)
            {
                if (_material == null) return;
            
                // Determine values
                var pixelationWidth = pixelation.widthPixelation.overrideState 
                    ? pixelation.widthPixelation.value 
                    : Screen.width;
                var pixelationHeight = pixelation.heightPixelation.overrideState 
                    ? pixelation.heightPixelation.value 
                    : Screen.height;
                var colorPrecision = pixelation.colorPrecision.overrideState 
                    ? pixelation.colorPrecision.value 
                    : int.MaxValue;
            
                // Apply to material
                _material.SetFloat(s_PixelationWidth, pixelationWidth);
                _material.SetFloat(s_PixelationHeight, pixelationHeight);
                _material.SetFloat(s_ColorPrecision, colorPrecision);
            }
        
            public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
            {
                // Fetch rendering data
                var resourceData = frameData.Get<UniversalResourceData>();      
                if (resourceData.isActiveTargetBackBuffer)
                {
                    Debug.LogError($"Skipping {nameof(PixelationRenderPass)}: An immediate ColorTexture is required. The BackBuffer cannot be used as a texture input.");
                    return;
                }
            
                // Check whether volume component exists and default settings are enabled
                var stack = VolumeManager.instance.stack;
                var pixelationVolume = stack.GetComponent<Pixelation>();
                if (!pixelationVolume.IsActive())
                    return;
            
                // Refresh the material values
                RefreshMaterialValues(pixelationVolume);
            
                // Get the current color texture
                var source = resourceData.activeColorTexture;
            
                // Fetch descriptor / parameters for the new RenderTexture
                _pixelationTextureDesc = renderGraph.GetTextureDesc(source);
                _pixelationTextureDesc.name = $"RT_{k_psxPassName}";
                _pixelationTextureDesc.filterMode = FilterMode.Point;
                _pixelationTextureDesc.clearBuffer = false;    // We want to modify not start from a blank slate.
            
                // Create the new render texture
                var destination = renderGraph.CreateTexture(_pixelationTextureDesc);
            
                // Blit the texture using the shader material
                var parameters = new RenderGraphUtils.BlitMaterialParameters(source, destination, _material, 0);
                renderGraph.AddBlitPass(parameters, passName: k_psxPassName);
            
                // Swap the camera color buffer with the new modified texture
                resourceData.cameraColor = destination;
            }
        }
    }
}


