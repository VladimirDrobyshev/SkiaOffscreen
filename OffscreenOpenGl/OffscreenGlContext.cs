using OffscreenOpenGl.Platform.Windows;

namespace OffscreenOpenGl;

public class OffscreenGlContext : IDisposable
{
    WglContext? _wglContext;
    
    public OffscreenGlContext(IntPtr hWnd)
    {
        //_wglContext = new WglContext(hWnd);
        _wglContext = new WglContext();
        _wglContext.MakeCurrent();
    }
    public void Dispose()
    {
        if (_wglContext != null)
        {
            //_wglContext.ReleaseCurrent();
            _wglContext.Dispose();
            _wglContext = null;
        }
    }
}