using SkiaSharp;

namespace SkiaSharpOffscreen.Models;

public class SkiaNativeModel : SkiaModelBase
{
    private SKSurface? _surface;

    protected override SKSurface GetSurface(int width, int height)
    {
        _surface ??= SKSurface.Create(new SKImageInfo(width, height));
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