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

    public IntPtr HWnd { get; set; }
    public IntPtr HInstance { get; set; }
    public ReactiveCommand<Unit, Unit> RenderSkiaCommand { get; }
    public ReactiveCommand<Unit, Unit> RenderVulkanCommand { get; }
    public ReactiveCommand<Unit, Unit> RenderOpenglCommand { get; }

    public SkiaViewModel()
    {
        RenderSkiaCommand = ReactiveCommand.Create(RenderSkia);
        RenderVulkanCommand = ReactiveCommand.Create(RenderVulkan);
        RenderOpenglCommand = ReactiveCommand.Create(RenderOpenGl);
    }
    void Render(SkiaModelBase model)
    {
        model.Render();
        Image = model.Image;
        Description = model.Description;
        model.Dispose();
    }

    void RenderSkia() => Render(new SkiaNativeModel());
    void RenderVulkan() => Render(new SkiaVulkanModel(HInstance, HWnd));
    void RenderOpenGl()
    {
    }
}
