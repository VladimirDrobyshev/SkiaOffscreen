using System.ComponentModel;
using System.Reactive;
using Avalonia.Media;
using ReactiveUI;
using SkiaSharpOffscreen.Models;

namespace SkiaSharpOffscreen.ViewModels;

public class SkiaViewModel : ViewModelBase
{
    private readonly RenderParams _params = new();

    private bool _fill = true;
    private bool _stroke = true;
    private int _width = 2000;
    private int _height = 1000;
    private int _primitiveCount = 100000;
    private int _primitiveSize = 20;
    private double _initTime;
    private double _renderTime;
    private double _presentTime;
    private IImage? _image;
    private PrimitiveType _primitiveType;
    private SkiaModelBase _renderModel = new SkiaNativeModel();

    public SkiaViewModel()
    {
        RenderCommand = ReactiveCommand.Create(Render);
        SetNativeCommand = ReactiveCommand.Create(() => SetRenderer(new SkiaNativeModel()));
        SetOpenGlCommand = ReactiveCommand.Create(() => SetRenderer(new SkiaOpenGlModel()));
        SetVulkanCommand = ReactiveCommand.Create(() => SetRenderer(new SkiaVulkanModel()));
        PropertyChanged += UpdateParams;
        UpdateParams(null, new PropertyChangedEventArgs(null));
    }

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
    
    public double PresentTime
    {
        get => _presentTime;
        set => this.RaiseAndSetIfChanged(ref _presentTime, value);
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

    private void UpdateParams(object? sender, PropertyChangedEventArgs e)
    {
        _params.Width = Width;
        _params.Height = Height;
        _params.PrimitiveSize = PrimitiveSize;
        _params.PrimitiveCount = PrimitiveCount;
        _params.Fill = Fill; //TODO VD: Add to settings
        _params.Stroke = Stroke; //TODO VD: Add to settings
        _params.PrimitiveType = PrimitiveType;

        if (e.PropertyName is nameof(Width) or nameof(Height))
        {
            _renderModel.Clear();
        }
    }

    private void Render()
    {
        _renderModel.Render(_params);
        Image = null;
        Image = _renderModel.Image;
        InitTime = _renderModel.InitTime;
        RenderTime = _renderModel.RenderTime;
        PresentTime = _renderModel.PresentTime;
    }

    private void SetRenderer(SkiaModelBase renderer)
    {
        _renderModel.Dispose();
        _renderModel = renderer;
    }
}