using System;
using OffscreenVulkan;
using SkiaSharp;

namespace SkiaSharpOffscreen.Models;

public class SkiaVulkanModel : SkiaModelBase
{
    OffscreenVkContext? _vkContext;
    GRContext? _grContext;
    SKSurface? _surface;

    protected override string RendererName => "Vulkan";

    protected override SKSurface GetSurface()
    {
        if (_surface == null)
        {
            _vkContext = new OffscreenVkContext();
            _grContext = GRContext.CreateVulkan(_vkContext.BackendContext);
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