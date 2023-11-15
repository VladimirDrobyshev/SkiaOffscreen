using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using SkiaSharp;

namespace SkiaSharpOffscreen.Models;

public abstract class SkiaModelBase : IDisposable
{
    public double InitTime { get; private set; }
    public double RenderTime { get; private set; }
    public IImage Image { get; private set; }

    void RenderPrimitives(SKCanvas canvas, RenderParams @params)
    {
        var rnd = new Random(0);

        for (int i = 0; i < @params.PrimitiveCount; i++)
        {
            using var paint = new SKPaint();
            paint.Style = SKPaintStyle.Stroke;
            paint.StrokeWidth = 1;
            paint.Style = @params.GetPaintStyle();
            paint.Color = RandomColor();

            switch (@params.PrimitiveType)
            {
                case PrimitiveType.Rectangle:
                    var location = RandomPoint();
                    canvas.DrawRect(location.X, location.Y, @params.PrimitiveSize, @params.PrimitiveSize, paint);
                    break;
                case PrimitiveType.Circle:
                    canvas.DrawCircle(RandomPoint(), @params.PrimitiveSize, paint);
                    break;
                case PrimitiveType.Line:
                    canvas.DrawLine(RandomPoint(), RandomPoint(), paint);
                    break;
            }
        }

        return;

        SKPoint RandomPoint() => new(rnd.Next(@params.Width - @params.PrimitiveSize),
            rnd.Next(@params.Height - @params.PrimitiveSize));

        SKColor RandomColor() => new((byte) rnd.Next(byte.MaxValue), (byte) rnd.Next(byte.MaxValue),
            (byte) rnd.Next(byte.MaxValue), (byte) rnd.Next(byte.MaxValue));
    }
    protected abstract SKSurface GetSurface(int width, int height);
    public void Render(RenderParams @params)
    {
        var stopwatchCanvas = Stopwatch.StartNew();
        var surface = GetSurface(@params.Width, @params.Height);
        surface.Canvas.Clear();
        stopwatchCanvas.Stop();
        
        var stopwatchRender = Stopwatch.StartNew();
        RenderPrimitives(surface.Canvas, @params);
        stopwatchRender.Stop();

        using var image = surface.Snapshot();
        using var imageData = image.Encode(); //TODO VD: here crashes vulkan on linux (or on any flush etc)
        using var stream = new MemoryStream();
        imageData.SaveTo(stream);
        stream.Seek(0, SeekOrigin.Begin);
        Image = new Bitmap(stream);
        
        InitTime = Math.Round(stopwatchCanvas.Elapsed.TotalMilliseconds, 2, MidpointRounding.AwayFromZero);
        RenderTime = Math.Round(stopwatchRender.Elapsed.TotalMilliseconds, 2, MidpointRounding.AwayFromZero);
    }
    public abstract void Dispose();
}