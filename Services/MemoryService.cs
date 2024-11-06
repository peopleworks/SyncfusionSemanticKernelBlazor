using Microsoft.KernelMemory;
using Microsoft.KernelMemory.DocumentStorage.DevTools;
using Microsoft.KernelMemory.FileSystem.DevTools;
using Microsoft.KernelMemory.MemoryStorage.DevTools;

namespace SyncfusionSemanticKernelBlazor.Services
{
    public class MemoryService : IMemoryService
    {
        public MemoryServerless BuildMemory()
        {
            var memoryConfiguration = new KernelMemoryConfig();
            var openAIConfig = new OpenAIConfig();

            new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .BindSection("KernelMemory", memoryConfiguration)
                .BindSection("KernelMemory:Services:OpenAI", openAIConfig);

            var builder = new KernelMemoryBuilder()
                .AddSingleton(memoryConfiguration)
                .WithOpenAI(openAIConfig)
                .WithSimpleFileStorage(new SimpleFileStorageConfig { StorageType = FileSystemTypes.Disk })
                .WithSimpleVectorDb(new SimpleVectorDbConfig { StorageType = FileSystemTypes.Disk });

            var s_memory = builder.Build<MemoryServerless>();
            return s_memory;
        }
    }
}
