using Microsoft.KernelMemory;

namespace SyncfusionSemanticKernelBlazor.Services
{
    public interface IMemoryService
    {
        MemoryServerless BuildMemory();
    }
}
