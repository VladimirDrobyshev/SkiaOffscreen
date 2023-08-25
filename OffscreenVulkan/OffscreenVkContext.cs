using SharpVk;
using SharpVk.Khronos;
using SkiaSharp;

namespace OffscreenVulkan;

public class OffscreenVkContext : IDisposable
{
    Instance? _instance;
    Surface? _surface;
    GRVkBackendContext? _backendContext;

    public GRVkBackendContext BackendContext => _backendContext!;

    public OffscreenVkContext(IntPtr hInstance, IntPtr hWnd)
    {
        _instance = Instance.Create(null, new[] { "VK_KHR_surface", "VK_KHR_win32_surface" }); //TODO VD: non windows
        var physicalDevice = _instance.EnumeratePhysicalDevices().First();

        _surface = _instance.CreateWin32Surface(hInstance, hWnd);
        
        var families = FindQueueFamilies(physicalDevice, _surface);

        var queueInfos = new[]
        {
            new DeviceQueueCreateInfo { QueueFamilyIndex = families.GraphicsFamily, QueuePriorities = new[] { 1f } },
            new DeviceQueueCreateInfo { QueueFamilyIndex = families.PresentFamily, QueuePriorities = new[] { 1f } },
        };
        var device = physicalDevice.CreateDevice(queueInfos, null, null);
        var graphicsQueue = device.GetQueue(families.GraphicsFamily, 0);
        _backendContext = new GRVkBackendContext
        {
            VkInstance = (IntPtr)_instance.RawHandle.ToUInt64(),
            VkPhysicalDevice = (IntPtr)physicalDevice.RawHandle.ToUInt64(),
            VkDevice = (IntPtr)device.RawHandle.ToUInt64(),
            VkQueue = (IntPtr)graphicsQueue.RawHandle.ToUInt64(),
            GraphicsQueueIndex = families.GraphicsFamily,
            GetProcedureAddress = (name, _, deviceHandle) =>
            {
                if (deviceHandle != IntPtr.Zero)
                    return device.GetProcedureAddress(name);
                return _instance.GetProcedureAddress(name);
            }
        };
    }
    static (uint GraphicsFamily, uint PresentFamily) FindQueueFamilies(PhysicalDevice physicalDevice, Surface surface)
    {
        var queueFamilyProperties = physicalDevice.GetQueueFamilyProperties();

        var graphicsFamily = queueFamilyProperties
            .Select((properties, index) => new { properties, index })
            .SkipWhile(pair => !pair.properties.QueueFlags.HasFlag(QueueFlags.Graphics))
            .FirstOrDefault();

        if (graphicsFamily == null)
            throw new Exception("Unable to find graphics queue");

        uint? presentFamily = default;
        
        for (uint i = 0; i < queueFamilyProperties.Length; ++i)
        {
            if (physicalDevice.GetSurfaceSupport(i, surface))
                presentFamily = i;
        }

        if (!presentFamily.HasValue)
            throw new Exception("Unable to find present queue");

        return ((uint)graphicsFamily.index, presentFamily.Value);
    }
    public void Dispose()
    {
        if (_backendContext != null)
        {
            _backendContext.Dispose();
            _backendContext = null;
        }
        if (_instance != null)
        {
            _instance.Dispose();
            _instance = null;
        }
    }
}