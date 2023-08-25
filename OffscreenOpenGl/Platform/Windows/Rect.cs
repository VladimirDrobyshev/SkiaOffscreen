using System.Runtime.InteropServices;

namespace OffscreenOpenGl.Platform.Windows;

[StructLayout(LayoutKind.Sequential)]
internal struct Rect
{
	public int left;
	public int top;
	public int right;
	public int bottom;
}

