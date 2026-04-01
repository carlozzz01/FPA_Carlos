using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering.RendererUtils;

[System.Serializable]
public class OutlineFullScreenPass : CustomPass
{
    public LayerMask outlineLayer;
    public Color outlineColor = Color.white;
    [Range(1, 10)]
    public float outlineWidth = 3f;
    public Material fullScreenMaterial;

    RTHandle silhouetteBuffer;
    Material silhouetteMaterial;

    protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
        silhouetteBuffer = RTHandles.Alloc(
            Vector2.one, TextureXR.slices,
            colorFormat: UnityEngine.Experimental.Rendering.GraphicsFormat.R8_UNorm,
            dimension: TextureXR.dimension,
            useDynamicScale: true,
            name: "SilhouetteBuffer"
        );

        silhouetteMaterial = new Material(Shader.Find("HDRP/Unlit"));
        silhouetteMaterial.SetColor("_UnlitColor", Color.white);
    }

    protected override void Execute(CustomPassContext ctx)
    {
        if (fullScreenMaterial == null) return;

        var shaderTags = new ShaderTagId[]
        {
            new ShaderTagId("Forward"),
            new ShaderTagId("ForwardOnly"),
            new ShaderTagId("SRPDefaultUnlit")
        };

        // Pass 1 — Renderiza silueta en blanco sobre negro
        CoreUtils.SetRenderTarget(ctx.cmd, silhouetteBuffer, ClearFlag.Color, Color.black);

        var descSilhouette = new RendererListDesc(shaderTags, ctx.cullingResults, ctx.hdCamera.camera)
        {
            rendererConfiguration     = PerObjectData.None,
            renderQueueRange          = RenderQueueRange.all,
            layerMask                 = outlineLayer,
            sortingCriteria           = SortingCriteria.CommonOpaque,
            overrideMaterial          = silhouetteMaterial,
            overrideMaterialPassIndex = 0,
            stateBlock                = new RenderStateBlock(RenderStateMask.Depth)
            {
                depthState = new DepthState(false, CompareFunction.Always)
            }
        };

        ctx.cmd.DrawRendererList(ctx.renderContext.CreateRendererList(descSilhouette));

        // Pass 2 — Full screen pass: detecta borde y dibuja outline
        fullScreenMaterial.SetTexture("_SilhouetteTexture", silhouetteBuffer);
        fullScreenMaterial.SetColor("_OutlineColor", outlineColor);
        fullScreenMaterial.SetFloat("_OutlineWidth", outlineWidth);

        CoreUtils.SetRenderTarget(ctx.cmd, ctx.cameraColorBuffer);
        CoreUtils.DrawFullScreen(ctx.cmd, fullScreenMaterial, shaderPassId: 0);
    }

    protected override void Cleanup()
    {
        RTHandles.Release(silhouetteBuffer);
        CoreUtils.Destroy(silhouetteMaterial);
    }
}