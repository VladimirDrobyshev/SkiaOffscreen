using System;
using OffscreenOpenGl;
using SkiaSharp;

namespace SkiaSharpOffscreen.Models;

public class SkiaOpenGlModel : SkiaModelBase
{
    readonly IntPtr _hWnd;
    OffscreenGlContext? _glContext;
    GRContext? _grContext;
    SKSurface? _surface;
    
    protected override string RendererName => "OpenGl";

    public SkiaOpenGlModel(IntPtr hWnd)
    {
        _hWnd = hWnd;
    }
    protected override SKSurface GetSurface()
    {
        if (_surface == null)
        {
            _glContext = new OffscreenGlContext(_hWnd);
            _grContext = GRContext.CreateGl();
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
        if (_glContext != null)
        {
            _glContext.Dispose();
            _glContext = null;
        }
    }
}