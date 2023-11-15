using System.ComponentModel;
using System.Reactive;
using Avalonia.Media;
using ReactiveUI;
using SkiaSharpOffscreen.Models;

namespace SkiaSharpOffscreen.ViewModels;

public class SkiaViewModel : ViewModelBase
{
    readonly RenderParams _params = new();

    bool _fill;
    bool _stroke = true;
    int _width = 2000;
    int _height = 1000;
    int _primitiveCount = 100000;
    int _primitiveSize = 20;
    double _initTime;
    double _renderTime;
    PrimitiveType _primitiveType;
    IImage? _image;
    SkiaModelBase _renderModel = new SkiaNativeModel();
    
    public bool Fill
    {
        get => _fill;
        set => this.RaiseAndSetIfChanged(ref _fill, value);
    }
    public bool Stroke
    {
        get => _stroke;
        set => this.RaiseAndSetIfChanged(ref _stroke, value);
    }
    public int Width
    {
        get => _width;
        set => this.RaiseAndSetIfChanged(ref _width, value);
    }
    public int Height
    {
        get => _height;
        set => this.RaiseAndSetIfChanged(ref _height, value);
    }
    public int PrimitiveCount
    {
        get => _primitiveCount;
        set => this.RaiseAndSetIfChanged(ref _primitiveCount, value);
    }
    public int PrimitiveSize
    {
        get => _primitiveSize;
        set => this.RaiseAndSetIfChanged(ref _primitiveSize, value);
    }
    public double InitTime
    {
        get => _initTime;
        set => this.RaiseAndSetIfChanged(ref _initTime, value);
    }
    public double RenderTime
    {
        get => _renderTime;
        set => this.RaiseAndSetIfChanged(ref _renderTime, value);
    }
    public PrimitiveType PrimitiveType
    {
        get => _primitiveType;
        set => this.RaiseAndSetIfChanged(ref _primitiveType, value);
    }
    public IImage? Image
    {
        get => _image;
        set => this.RaiseAndSetIfChanged(ref _image, value);
    }
    
    public ReactiveCommand<Unit, Unit> RenderCommand { get; }
    public ReactiveCommand<Unit, Unit> SetNativeCommand { get; }
    public ReactiveCommand<Unit, Unit> SetOpenGlCommand { get; }
    public ReactiveCommand<Unit, Unit> SetVulkanCommand { get; }

    public SkiaViewModel()
    {
        RenderCommand = ReactiveCommand.Create(Render);
        SetNativeCommand = ReactiveCommand.Create(() => SetRenderer(new SkiaNativeModel()));
        SetOpenGlCommand = ReactiveCommand.Create(() => SetRenderer(new SkiaOpenGlModel()));
        SetVulkanCommand = ReactiveCommand.Create(() => SetRenderer(new SkiaVulkanModel()));
        PropertyChanged += UpdateParams;
        UpdateParams(null, new PropertyChangedEventArgs(null));
    }

    void UpdateParams(object? sender, PropertyChangedEventArgs e)
    {
        _params.Width = Width;
        _params.Height = Height;
        _params.PrimitiveSize = PrimitiveSize;
        _params.PrimitiveCount = PrimitiveCount;
        _params.Fill = Fill; //TODO VD: Add to settings
        _params.Stroke = Stroke; //TODO VD: Add to settings
        //TODO VD: Add to settings
        //_params.PrimitiveType
    }

    void Render()
    {
        _renderModel.Render(_params);
        Image = _renderModel.Image;
        InitTime = _renderModel.InitTime;
        RenderTime = _renderModel.RenderTime;
    }
    void SetRenderer(SkiaModelBase renderer)
    {
        _renderModel.Dispose();
        _renderModel = renderer;
    }
}
