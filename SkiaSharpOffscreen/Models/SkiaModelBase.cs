using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Avalonia.Media;
using SkiaSharp;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

namespace SkiaSharpOffscreen.Models;

public abstract class SkiaModelBase : IDisposable
{
    const int PrimitivesCount = 1000000;
    protected const int Width = 800;
    protected const int Height = 600;
    
    protected abstract string RendererName { get; }

    public string Description { get; private set; }
    public IImage Image { get; private set; }

    void RenderPrimitives(SKCanvas canvas)
    {
        const int maxWidth = Width / 4;
        const int maxHeight = Height / 4;
        var rnd = new Random(0);

        canvas.Clear(SKColors.SkyBlue);
        for (int i = 0; i < PrimitivesCount; i++)
        {
            using var paint = new SKPaint();
            paint.Style = SKPaintStyle.Stroke;
            paint.StrokeWidth = 1;
            paint.Color = new SKColor((byte) rnd.Next(byte.MaxValue), (byte) rnd.Next(byte.MaxValue),
                (byte) rnd.Next(byte.MaxValue), (byte) rnd.Next(byte.MaxValue));

            if (i % 3 == 0)
                canvas.DrawLine(RandomPoint(), RandomPoint(), paint);
            else if (i % 3 == 1)
                canvas.DrawCircle(RandomPoint(), rnd.Next(maxWidth), paint);
            else
                canvas.DrawRect(rnd.Next(maxWidth), rnd.Next(maxHeight), rnd.Next(maxWidth), rnd.Next(maxHeight), paint);
        }

        return;

        SKPoint RandomPoint() => new (rnd.Next(Width), rnd.Next(Height));
    }
    protected abstract SKSurface GetSurface();
    public void Render()
    {
        var stopwatchCanvas = Stopwatch.StartNew();
        var surface = GetSurface();
        stopwatchCanvas.Stop();
        
        var stopwatchRender = Stopwatch.StartNew();
        RenderPrimitives(surface.Canvas);
        stopwatchRender.Stop();

        using var image = surface.Snapshot();
        using var imageData = image.Encode();
        using var stream = new MemoryStream();
        imageData.SaveTo(stream);
        stream.Seek(0, SeekOrigin.Begin);
        Image = new Bitmap(stream);
        
        var output = new StringBuilder();
        output.AppendLine($"Backend: {RendererName}");
        output.AppendLine($"Primitives count: {PrimitivesCount}");
        output.AppendLine($"Render time: {Math.Ceiling(stopwatchRender.Elapsed.TotalMilliseconds)}ms");
        output.AppendLine($"Surface creation time: {stopwatchCanvas.Elapsed.TotalMilliseconds}ms");
        Description = output.ToString();
    }
    public abstract void Dispose();
}