using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;

namespace SyncfusionSemanticKernelBlazor.Services
{
    public interface IMarkdownAssistant
    {

        private static Kernel? kernel;
        private static IKernelMemory memoryConnector;
        private static KernelFunction myFunction;
        private static IMemoryService memoryService;
        public static string emptyMessage = "I don't have an answer to that question";
        public void CreateMarkdownAssistant();

        public void LoadDocument(string documentId);

        public Task<string> AskQuestionAsync(string question);
    }
}
