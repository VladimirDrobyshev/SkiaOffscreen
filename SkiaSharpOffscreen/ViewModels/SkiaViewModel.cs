using System;
using System.Reactive;
using Avalonia.Media;
using ReactiveUI;
using SkiaSharpOffscreen.Models;

namespace SkiaSharpOffscreen.ViewModels;

public class SkiaViewModel : ViewModelBase
{
    string _description = "Press any button";
    IImage? _image;
    
    public string Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }
    public IImage? Image
    {
        get => _image;
        set => this.RaiseAndSetIfChanged(ref _image, value);
    }
    
    public ReactiveCommand<Unit, Unit> RenderSkiaCommand { get; }
    public ReactiveCommand<Unit, Unit> RenderVulkanCommand { get; }
    public ReactiveCommand<Unit, Unit> RenderOpenGlCommand { get; }

    public SkiaViewModel()
    {
        RenderSkiaCommand = ReactiveCommand.Create(() => Render(new SkiaNativeModel()));
        RenderVulkanCommand = ReactiveCommand.Create(() => Render(new SkiaVulkanModel()));
        RenderOpenGlCommand = ReactiveCommand.Create(() => Render(new SkiaOpenGlModel()));
    }
    void Render(SkiaModelBase model)
    {
        try
        {
            model.Render();
            Image = model.Image;
            Description = model.Description;
        }
        finally
        {
            model.Dispose();
        }
    }
}
