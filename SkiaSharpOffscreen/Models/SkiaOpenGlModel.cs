using System;
using OffscreenOpenGl;
using SkiaSharp;

namespace SkiaSharpOffscreen.Models;

public class SkiaOpenGlModel : SkiaModelBase
{
    OffscreenGlContext? _glContext;
    GRContext? _grContext;
    SKSurface? _surface;
    
    protected override string RendererName => "OpenGl";

    protected override SKSurface GetSurface()
    {
        if (_surface == null)
        {
            _glContext = new OffscreenGlContext();
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