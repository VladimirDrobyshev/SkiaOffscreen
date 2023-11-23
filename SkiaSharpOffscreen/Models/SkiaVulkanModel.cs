using OffscreenVulkan;
using SkiaSharp;

namespace SkiaSharpOffscreen.Models;

public class SkiaVulkanModel : SkiaModelBase
{
    private GRContext? _grContext;
    private SKSurface? _surface;
    private OffscreenVkContext? _vkContext;

    protected override SKSurface GetSurface(int width, int height)
    {
        if (_surface == null)
        {
            _vkContext = new OffscreenVkContext();
            _grContext = GRContext.CreateVulkan(_vkContext.BackendContext);
            _surface = SKSurface.Create(_grContext, true, new SKImageInfo(width, height));
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

        if (_grContext != null)
        {
            _grContext.Dispose();
            _grContext = null;
        }

        if (_vkContext != null)
        {
            _vkContext.Dispose();
            _vkContext = null;
        }
    }
}