using SkiaSharp;

namespace SkiaSharpOffscreen.Models;

public class SkiaNativeModel : SkiaModelBase
{
    SKSurface? _surface;

    protected override string RendererName => "Native Skia";

    protected override SKSurface GetSurface()
    {
        _surface ??= SKSurface.Create(new SKImageInfo(Width, Height));
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