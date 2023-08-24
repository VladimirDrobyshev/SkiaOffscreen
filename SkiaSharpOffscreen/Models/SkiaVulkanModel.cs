using OffscreenVulkan;
using SkiaSharp;

namespace SkiaSharpOffscreen.Models;

public class SkiaVulkanModel : SkiaModelBase
{
    OffscreenVkContext? _offscreenVkContext;
    GRContext? _grContext;
    SKSurface? _surface;

    protected override string RendererName => "Vulkan";

    protected override SKSurface GetSurface()
    {
        if (_surface == null)
        {
            _offscreenVkContext = new OffscreenVkContext(Width, Height);
            _grContext = GRContext.CreateVulkan(_offscreenVkContext.BackendContext);
            _surface = SKSurface.Create(_grContext, true, new SKImageInfo(Width, Height));
        }
        return _surface;
    }
    public override void Dispose()
    {
        if (_surface != null)
        {
            _surface.Dispose();
            _surface = null;
        }
    }
}