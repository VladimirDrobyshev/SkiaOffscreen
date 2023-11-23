using System;
using SkiaSharp;

namespace SkiaSharpOffscreen.Models;

public class RenderParams
{
    public bool Stroke { get; set; }
    public bool Fill { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int PrimitiveCount { get; set; }
    public int PrimitiveSize { get; set; }
    public int Seed => new Random().Next();
    public PrimitiveType PrimitiveType { get; set; }

    public SKPaintStyle GetPaintStyle()
    {
        if (Stroke && Fill)
            return SKPaintStyle.StrokeAndFill;
        if (Stroke)
            return SKPaintStyle.Stroke;
        return SKPaintStyle.Fill;
    }
}