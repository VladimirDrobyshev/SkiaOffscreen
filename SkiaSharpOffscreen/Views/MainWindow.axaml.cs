using System;
using Avalonia.Controls;
using Avalonia.VisualTree;
using Avalonia.Win32;
using SkiaSharpOffscreen.ViewModels;

namespace SkiaSharpOffscreen.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        if (DataContext is SkiaViewModel viewModel)
        {
            var toplevel = (TopLevel)this.GetVisualRoot();
            var platformImpl = (WindowImpl)toplevel.PlatformImpl;
            viewModel.HWnd = platformImpl.Handle.Handle;
        }
        base.OnDataContextChanged(e);
    }
}
