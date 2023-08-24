using Avalonia.Controls;
using Avalonia.Interactivity;

namespace SkiaSharpOffscreen;

public partial class MainWindow : Window
{
    
    public MainWindow()
    {
        InitializeComponent();
    }

    void RenderSkia(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    void RenderOpenGl(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    void RenderVulkan(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}
