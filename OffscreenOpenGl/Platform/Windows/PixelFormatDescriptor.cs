﻿using System.Runtime.InteropServices;

namespace OffscreenOpenGl.Platform.Windows;

[StructLayout(LayoutKind.Sequential)]
internal struct PixelFormatDescriptor
{
	public ushort nSize;
	public ushort nVersion;
	public uint dwFlags;
	public byte iPixelType;
	public byte cColorBits;
	public byte cRedBits;
	public byte cRedShift;
	public byte cGreenBits;
	public byte cGreenShift;
	public byte cBlueBits;
	public byte cBlueShift;
	public byte cAlphaBits;
	public byte cAlphaShift;
	public byte cAccumBits;
	public byte cAccumRedBits;
	public byte cAccumGreenBits;
	public byte cAccumBlueBits;
	public byte cAccumAlphaBits;
	public byte cDepthBits;
	public byte cStencilBits;
	public byte cAuxBuffers;
	public byte iLayerType;
	public byte bReserved;
	public int dwLayerMask;
	public int dwVisibleMask;
	public int dwDamageMask;
}