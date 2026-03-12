using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace Karbon {

sealed class CompositePass : ScriptableRenderPass
{
    sealed class PassData { public Material Material; }

    public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
    {
        var camera = frameData.Get<UniversalCameraData>().camera;
        var controller = camera.GetComponent<CompositeController>();
        if (controller == null || !controller.enabled || !controller.IsActive) return;

        var resourceData = frameData.Get<UniversalResourceData>();
        if (resourceData.isActiveTargetBackBuffer) return;

        var source = resourceData.activeColorTexture;
        if (!source.IsValid()) return;

        var mat = controller.UpdateMaterial();
        if (mat == null) return;

        using var builder =
          renderGraph.AddRasterRenderPass<PassData>("Composite", out var passData);
        passData.Material = mat;

        builder.SetRenderAttachment(source, 0, AccessFlags.Write);
        builder.SetRenderFunc<PassData>(ExecutePass);
    }

    static void ExecutePass(PassData data, RasterGraphContext context)
      => CoreUtils.DrawFullScreen(context.cmd, data.Material);
}

} // namespace Karbon
