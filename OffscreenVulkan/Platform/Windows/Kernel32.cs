using System.Runtime.InteropServices;

namespace OffscreenVulkan.Platform.Windows;

internal static class Kernel32
{
    static Kernel32()
    {
        CurrentModuleHandle = GetModuleHandle(null);
        if (CurrentModuleHandle == IntPtr.Zero)
        {
            throw new Exception("Could not get module handle.");
        }
    }

    public static IntPtr CurrentModuleHandle { get; }

    [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
    static extern IntPtr GetModuleHandle([MarshalAs(UnmanagedType.LPTStr)] string lpModuleName);
}