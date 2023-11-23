using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using SkiaSharp;

namespace SkiaSharpOffscreen.Models;

public abstract class SkiaModelBase : IDisposable
{
    public double InitTime { get; private set; }
    public double RenderTime { get; private set; }
    public double PresentTime { get; private set; }
    public WriteableBitmap? Image { get; private set; }
    public abstract void Dispose();

    private void RenderPrimitives(SKCanvas canvas, RenderParams @params)
    {
        var rnd = new Random(@params.Seed);

        for (var i = 0; i < @params.PrimitiveCount; i++)
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

        SKPoint RandomPoint()
        {
            return new SKPoint(rnd.Next(@params.Width - @params.PrimitiveSize),
                rnd.Next(@params.Height - @params.PrimitiveSize));
        }

        SKColor RandomColor()
        {
            return new SKColor((byte)rnd.Next(byte.MaxValue), (byte)rnd.Next(byte.MaxValue),
                (byte)rnd.Next(byte.MaxValue), (byte)rnd.Next(byte.MaxValue));
        }
    }

    protected abstract SKSurface GetSurface(int width, int height);

    protected abstract void DestroySurface();

    public void Render(RenderParams @params)
    {
        var stopwatchCanvas = Stopwatch.StartNew();
        var surface = GetSurface(@params.Width, @params.Height);
        surface.Canvas.Clear();
        stopwatchCanvas.Stop();

        var stopwatchRender = Stopwatch.StartNew();
        RenderPrimitives(surface.Canvas, @params);
        stopwatchRender.Stop();

        var stopwatchPresent = Stopwatch.StartNew();
        Image ??= new WriteableBitmap(new PixelSize(@params.Width, @params.Height), new Vector(96, 96), PixelFormat.Bgra8888, AlphaFormat.Premul);
        using (var framebuffer = Image.Lock())
        {
            surface.ReadPixels(new SKImageInfo(@params.Width, @params.Height, SKColorType.Bgra8888, SKAlphaType.Premul), framebuffer.Address, framebuffer.RowBytes, 0, 0);
        }
        stopwatchPresent.Stop();

        InitTime = Math.Round(stopwatchCanvas.Elapsed.TotalMilliseconds, 2, MidpointRounding.AwayFromZero);
        RenderTime = Math.Round(stopwatchRender.Elapsed.TotalMilliseconds, 2, MidpointRounding.AwayFromZero);
        PresentTime = Math.Round(stopwatchPresent.Elapsed.TotalMilliseconds, 2, MidpointRounding.AwayFromZero);
    }

    public void Clear()
    {
        DestroySurface();
        Image?.Dispose();
        Image = null;
    }
}