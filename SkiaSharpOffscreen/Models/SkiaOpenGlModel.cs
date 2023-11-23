using OffscreenOpenGl;
using SkiaSharp;

namespace SkiaSharpOffscreen.Models;

public class SkiaOpenGlModel : SkiaModelBase
{
    private OffscreenGlContext? _glContext;
    private GRContext? _grContext;
    private SKSurface? _surface;

    protected override SKSurface GetSurface(int width, int height)
    {
        if (_surface == null)
        {
            _glContext ??= new OffscreenGlContext();
            _grContext ??= GRContext.CreateGl();
            _surface = SKSurface.Create(_grContext, true, new SKImageInfo(width, height));
        }

        return _surface;
    }

    protected override void DestroySurface()
    {
        if (_surface != null)
        {
            _surface.Dispose();
            _surface = null;
        }
    }

    public override void Dispose()
    {
        DestroySurface();

        if (_glContext != null)
        {
            _glContext.Dispose();
            _glContext = null;
        }
    }
}