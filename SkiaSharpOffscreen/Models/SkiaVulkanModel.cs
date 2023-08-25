using System;
using OffscreenVulkan;
using SkiaSharp;

namespace SkiaSharpOffscreen.Models;

public class SkiaVulkanModel : SkiaModelBase
{
    readonly IntPtr _hInstance;
    readonly IntPtr _hWnd;
    OffscreenVkContext? _offscreenVkContext;
    GRContext? _grContext;
    SKSurface? _surface;

    protected override string RendererName => "Vulkan";

    public SkiaVulkanModel(IntPtr hInstance, IntPtr hWnd)
    {
        _hInstance = hInstance;
        _hWnd = hWnd;
    }
    protected override SKSurface GetSurface()
    {
        if (_surface == null)
        {
            _offscreenVkContext = new OffscreenVkContext(_hInstance, _hWnd);
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