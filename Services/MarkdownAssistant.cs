using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SyncfusionSemanticKernelBlazor.Helper;
using SyncfusionSemanticKernelBlazor.Services;
using System;

#pragma warning disable
public class MarkdownAssistant 
{
    private readonly Kernel kernel;
    private readonly IKernelMemory memoryConnector;
    private readonly KernelFunction myFunction;
    private static IMemoryService memoryService;
    public static string emptyMessage = "I don't have an answer to that question";

    public MarkdownAssistant()
    {
        var basePath = System.IO.Directory.GetCurrentDirectory();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();


        var builder = Kernel.CreateBuilder();
        builder
        .AddOpenAIChatCompletion(
            modelId: configuration["KernelMemory:Services:OpenAI:TextModel"],
            apiKey: configuration["KernelMemory:Services:OpenAI:APIKey"],
            httpClient: HttpLogger.GetHttpClient(false));

        // Create a kernel and connect it to the selected service
        kernel = builder.Build();


        var promptOptions = new OpenAIPromptExecutionSettings
        {
            ChatSystemPrompt =
                $"You are a document analyzer. Your task is to analyze the provided document generate a respond. Always respond in proper HTML format, but do not include <html>, <head>, or <body> tags.",
            ModelId = configuration["KernelMemory:Services:OpenAI:TextModel"],
            MaxTokens = 2048,
            Temperature = 0.5,
            TopP = 0
        };


        var skPrompt = """
                        Question: {{$input}}
                        Tool call result: {{memory.ask $input}}
                        If the answer is empty say "{{emptyMessage}}".
                        """;

        myFunction = kernel.CreateFunctionFromPrompt(skPrompt, promptOptions);

        memoryService = new SyncfusionSemanticKernelBlazor.Services.MemoryService();

        memoryConnector = memoryService.BuildMemory();

        // Add Plugin
        var memoryPlugin = kernel.ImportPluginFromObject(
            new MemoryPlugin(memoryConnector, waitForIngestionToComplete: true),
            "memory");
    }

    public async void LoadDocument(string documentId)
    {
        if(!await memoryConnector.IsDocumentReadyAsync(documentId: documentId))
        {
            await memoryConnector.ImportDocumentAsync(
                filePath: @$"wwwroot\docs\{documentId}.md",
                documentId: documentId);
        }
    }

    public async Task<string> AskQuestionAsync(string question)
    {
        try
        {
            var answer = await myFunction.InvokeAsync(kernel, question);
            return answer.GetValue<string>();
        } catch(Exception ex)
        {
            Console.WriteLine(ex.Message);

            return string.Empty;
        }
    }
}