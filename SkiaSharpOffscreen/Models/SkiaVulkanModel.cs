using SkiaSharp;

namespace SkiaSharpOffscreen.Models;

public class SkiaVulkanModel : SkiaModelBase
{
    SKSurface? _surface;

    protected override string RendererName => "Vulkan";

    protected override SKSurface GetSurface()
    {
        if (_surface == null)
        {

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