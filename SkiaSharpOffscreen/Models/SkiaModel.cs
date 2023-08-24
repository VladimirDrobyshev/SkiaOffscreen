using SkiaSharp;

namespace SkiaSharpOffscreen.Models;

public class SkiaModel : SkiaModelBase
{
    readonly SKSurface _surface;
    
    protected override string RendererName => "Native Skia";

    public SkiaModel()
    {
        _surface = SKSurface.Create(new SKImageInfo(Width, Height));
    }
    protected override SKCanvas GetCanvas() => _surface.Canvas;
    protected override SKImage Snapshot() => _surface.Snapshot();
    public override void Dispose()
    {
        _surface.Dispose();
    }
}