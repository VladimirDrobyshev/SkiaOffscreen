using SharpVk;
using SkiaSharp;

namespace OffscreenVulkan;

public class OffscreenVkContext : IDisposable
{
    Instance? _instance;
    Device? _device;
    GRVkBackendContext? _backendContext;

    public GRVkBackendContext BackendContext => _backendContext!;

    public OffscreenVkContext()
    {
        _instance = Instance.Create(null, null);
        var physicalDevice = _instance.EnumeratePhysicalDevices().First();

        uint graphicsQueueIndex = FindQueueFamilies(physicalDevice);

        var queueInfos = new[]
        {
            new DeviceQueueCreateInfo { QueueFamilyIndex = graphicsQueueIndex, QueuePriorities = new[] { 1f } }
        };
        _device = physicalDevice.CreateDevice(queueInfos, null, null);
        var graphicsQueue = _device.GetQueue(graphicsQueueIndex, 0);
        _backendContext = new GRVkBackendContext
        {
            VkInstance = (IntPtr)_instance.RawHandle.ToUInt64(),
            VkPhysicalDevice = (IntPtr)physicalDevice.RawHandle.ToUInt64(),
            VkDevice = (IntPtr)_device.RawHandle.ToUInt64(),
            VkQueue = (IntPtr)graphicsQueue.RawHandle.ToUInt64(),
            GraphicsQueueIndex = graphicsQueueIndex,
            GetProcedureAddress = (name, _, deviceHandle) =>
            {
                var result = IntPtr.Zero;
                if (deviceHandle != IntPtr.Zero)
                    result = _device.GetProcedureAddress(name);
                if (result == IntPtr.Zero)
                    result = _instance.GetProcedureAddress(name);
                return result;
            },
            Extensions = null,
            ProtectedContext = false,
            VkPhysicalDeviceFeatures = IntPtr.Zero,
            VkPhysicalDeviceFeatures2 = IntPtr.Zero,
            MaxAPIVersion = 0
        };
    }
    static uint FindQueueFamilies(PhysicalDevice physicalDevice)
    {
        var queueFamilyProperties = physicalDevice.GetQueueFamilyProperties();

        for (uint i = 0; i < queueFamilyProperties.Length; i++)
        {
            if (queueFamilyProperties[i].QueueFlags.HasFlag(QueueFlags.Graphics))
                return i;
        }

        throw new Exception("Unable to find graphics queue");
    }
    public void Dispose()
    {
        if (_backendContext != null)
        {
            _backendContext.Dispose();
            _backendContext = null;
        }
        
        if (_device != null)
        {
            _device.Dispose();
            _device = null;
        }

        if (_instance != null)
        {
            _instance.Dispose();
            _instance = null;
        }
    }
}