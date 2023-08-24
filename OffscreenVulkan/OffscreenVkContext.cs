using SharpVk;
using SharpVk.Khronos;
using SkiaSharp;

namespace OffscreenVulkan;

public class OffscreenVkContext : IDisposable
{
    Instance? _instance;
    GRVkBackendContext? _backendContext;

    public GRVkBackendContext BackendContext => _backendContext!;

    public OffscreenVkContext(int width, int height)
    {
        _instance = Instance.Create(null, null);
        var physicalDevice = _instance.EnumeratePhysicalDevices().First();

        // var displayMode = physicalDevice.CreateDisplayMode(physicalDevice.GetDisplayPlaneSupportedDisplays(0).First(), new DisplayModeParameters());
        // var surface = _instance.CreateDisplayPlaneSurface(displayMode, 0 , 0, SurfaceTransformFlags.Identity, 0, DisplayPlaneAlphaFlags.None, new Extent2D((uint)width, (uint)height));
        
        var families = FindQueueFamilies(physicalDevice/*, surface*/);

        var queueInfos = new[]
        {
            new DeviceQueueCreateInfo { QueueFamilyIndex = families.GraphicsFamily, QueuePriorities = new[] { 1f } },
            new DeviceQueueCreateInfo { QueueFamilyIndex = families.PresentFamily, QueuePriorities = new[] { 1f } },
        };
        var device = physicalDevice.CreateDevice(queueInfos, null, null);
        // var renderPass = device.CreateRenderPass(null, null, null);
        // _framebuffer = device.CreateFramebuffer(renderPass, null, (uint)width, (uint)height, 0);
        var graphicsQueue = device.GetQueue(families.GraphicsFamily, 0);
        //var presentQueue = device.GetQueue(families.PresentFamily, 0);
        _backendContext = new GRSharpVkBackendContext
        {
            VkInstance = _instance,
            VkPhysicalDevice = physicalDevice,
            VkDevice = device,
            VkQueue = graphicsQueue,
            GraphicsQueueIndex = families.GraphicsFamily,
            GetProcedureAddress = (name, instance, funcDevice) =>
            {
                if (funcDevice != null)
                    return funcDevice.GetProcedureAddress(name);
                return instance?.GetProcedureAddress(name) ?? _instance.GetProcedureAddress(name);
            },
            VkPhysicalDeviceFeatures = physicalDevice.GetFeatures(),
        };
    }
    static (uint GraphicsFamily, uint PresentFamily) FindQueueFamilies(PhysicalDevice physicalDevice/*, Surface surface*/)
    {
        var queueFamilyProperties = physicalDevice.GetQueueFamilyProperties();

        var graphicsFamily = queueFamilyProperties
            .Select((properties, index) => new { properties, index })
            .SkipWhile(pair => !pair.properties.QueueFlags.HasFlag(QueueFlags.Graphics))
            .FirstOrDefault();

        if (graphicsFamily == null)
            throw new Exception("Unable to find graphics queue");

        uint? presentFamily = 0;
        // uint? presentFamily = default;
        //
        // for (uint i = 0; i < queueFamilyProperties.Length; ++i)
        // {
        //     if (physicalDevice.GetSurfaceSupport(i, surface))
        //         presentFamily = i;
        // }

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
        // if (_framebuffer != null)
        // {
        //     _framebuffer.Destroy();
        //     _framebuffer.Dispose();
        //     _framebuffer = null;
        // }
        // if (_renderPass != null)
        // {
        //     _renderPass.Destroy();
        //     _renderPass.Dispose();
        //     _renderPass = null;
        // }
        // if (_device != null)
        // {
        //     _device.Destroy();
        //     _device.Dispose();
        //     _device = null;
        // }
        if (_instance != null)
        {
            _instance.Dispose();
            _instance = null;
        }
    }
}