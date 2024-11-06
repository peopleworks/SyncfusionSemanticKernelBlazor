using Azure;
using Microsoft.JSInterop;
using Microsoft.KernelMemory;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.InteractiveChat;
using Syncfusion.Blazor.SfPdfViewer;
using SyncfusionSemanticKernelBlazor.Helper;
using SyncfusionSemanticKernelBlazor.Services;
using System.Text.RegularExpressions;


namespace SyncfusionSemanticKernelBlazor.Components.Pages
{
    //HACK:(Kernel Memory (KM)) https://microsoft.github.io/kernel-memory/
    //HACK:(Syncfusion Blazor AI) https://github.com/syncfusion/smart-ai-samples/blob/master/blazor/README.md

    public partial class Home
    {
        // Instantiate the service and build the memory
        private static IMemoryService memoryService = new SyncfusionSemanticKernelBlazor.Services.MemoryService();
        private static MemoryServerless? s_memory = memoryService.BuildMemory();
        private static readonly List<string> s_toDelete = new();
        #region Grid : List of PDF's and Popup Settings
        SfPdfViewer2 viewer;

        bool dialogVisible { get; set; } = false;

        private string DocumentPath { get; set; } = string.Empty;

        private string HeaderText;

        bool enableStickyNotesAnnotation { get; set; } = true;

        bool isDialogOpended { get; set; } = false;

        PdfViewerAnnotationSettings annotationSettings;
        PdfViewerToolbarSettings ToolbarSettings;
        PdfViewerContextMenuSettings ContextMenuSettings;

        bool enableTextSelection { get; set; } = false;

        bool isReadOnlyMode { get; set; } = false;

        private async Task DocumentLoaded(LoadEventArgs args)
        {
            // 1. Clean
            //reset to initial state
            isDocumentLoaded = false;
            isSummarized = false;
            isDocumentLoadedInAI = false;
            isPopupVisible = false;
            _prompts?.Clear();
            _promptSuggestions?.Clear();

            // 2. Review Doc
            if(isReadOnlyMode)
            {
                List<FormField> fields = await viewer.RetrieveFormFieldsAsync();
                for(int i = 0; i < fields.Count(); i++)
                {
                    fields[i].IsReadOnly = true;
                    await viewer.UpdateFormFieldsAsync(fields[i]);
                }
            }

            // 3. Store the document (Vector Store)

            string docName = $"wwwroot\\data\\{args.DocumentName}";
            string documentId = DocumentIdGenerator.GenerateDocumentId(args.DocumentName);

            await StoreFile(docName, args.DocumentName);

            isDocumentLoaded = true;
        }

        private void OnOpen() { isDialogOpended = true; }

        private void OnClose()
        {
            isDialogOpended = false;
            // Clean Document
            CleanAndOpenNewDocument();
        }

        private void CommandClick(CommandClickEventArgs<Data> args)
        {
            if(args.CommandColumn.ButtonOption.IconCss == "e-icons e-eye")
            {
                this.dialogVisible = true;
                HeaderText = args.RowData.FileName;
                DocumentPath = "wwwroot/data/" + args.RowData.FilePath;
                annotationSettings = new PdfViewerAnnotationSettings() { IsLock = true };
                ToolbarSettings = new PdfViewerToolbarSettings()
                {
                    ToolbarItems =
                        new List<Syncfusion.Blazor.SfPdfViewer.ToolbarItem>()
                        {
                            Syncfusion.Blazor.SfPdfViewer.ToolbarItem.OpenOption,
                            Syncfusion.Blazor.SfPdfViewer.ToolbarItem.PageNavigationTool,
                            Syncfusion.Blazor.SfPdfViewer.ToolbarItem.MagnificationTool,
                            Syncfusion.Blazor.SfPdfViewer.ToolbarItem.PanTool,
                            Syncfusion.Blazor.SfPdfViewer.ToolbarItem.PrintOption,
                        },
                    MobileToolbarItems =
                        new List<MobileToolbarItem>() { MobileToolbarItem.Open, MobileToolbarItem.Search }
                };

                ContextMenuSettings = new PdfViewerContextMenuSettings() { EnableContextMenu = false };

                enableStickyNotesAnnotation = false;
                enableTextSelection = false;
                isReadOnlyMode = true;
            } else if(args.CommandColumn.ButtonOption.IconCss == "e-icons e-edit")
            {
                this.dialogVisible = true;
                HeaderText = args.RowData.FileName;
                // Sets the PDF document path for initial loading.
                DocumentPath = "wwwroot/data/" + args.RowData.FilePath;

                annotationSettings = new PdfViewerAnnotationSettings() { IsLock = false };
                ToolbarSettings = new PdfViewerToolbarSettings()
                {
                    ToolbarItems =
                        new List<Syncfusion.Blazor.SfPdfViewer.ToolbarItem>()
                        {
                            Syncfusion.Blazor.SfPdfViewer.ToolbarItem.OpenOption,
                            Syncfusion.Blazor.SfPdfViewer.ToolbarItem.PageNavigationTool,
                            Syncfusion.Blazor.SfPdfViewer.ToolbarItem.MagnificationTool,
                            Syncfusion.Blazor.SfPdfViewer.ToolbarItem.SelectionTool,
                            Syncfusion.Blazor.SfPdfViewer.ToolbarItem.PanTool,
                            Syncfusion.Blazor.SfPdfViewer.ToolbarItem.UndoRedoTool,
                            Syncfusion.Blazor.SfPdfViewer.ToolbarItem.CommentTool,
                            Syncfusion.Blazor.SfPdfViewer.ToolbarItem.SearchOption,
                            Syncfusion.Blazor.SfPdfViewer.ToolbarItem.AnnotationEditTool,
                            Syncfusion.Blazor.SfPdfViewer.ToolbarItem.PrintOption,
                            Syncfusion.Blazor.SfPdfViewer.ToolbarItem.DownloadOption
                        },
                    MobileToolbarItems =
                        new List<MobileToolbarItem>()
                        {
                            MobileToolbarItem.Open,
                            MobileToolbarItem.UndoRedo,
                            MobileToolbarItem.EditAnnotation,
                            MobileToolbarItem.Search
                        }
                };
                ContextMenuSettings = new PdfViewerContextMenuSettings() { EnableContextMenu = true };
                enableStickyNotesAnnotation = true;
                enableTextSelection = true;
                isReadOnlyMode = false;
            }
        }

        protected override void OnInitialized()
        {
            DataItem = Enumerable.Range(1, 5)
                .Select(x => new Data() { FileName = GetFileName(x), FilePath = GetFilePath(x), Author = GetAuthor(x) })
                .ToList();
        }

        public List<Data> DataItem { get; set; }

        private string GetFileName(int index)
        {
            string[] fileNames =
            {
                "PDF Succinctly.pdf",
                "Hive Succinctly.pdf",
                "GIS Succinctly.pdf",
                "JavaScript Succinctly.pdf",
                "HTTP Succinctly.pdf"
            };

            if(index >= 1 && index <= fileNames.Length)
            {
                return fileNames[index - 1];
            }
            return string.Empty;
        }

        private string GetAuthor(int index)
        {
            string[] fileNames = { "Ryan Hodson", "Elton Stoneman", "Peter Shaw", "Cody Lindley", "Scott Allen" };

            if(index >= 1 && index <= fileNames.Length)
            {
                return fileNames[index - 1];
            }
            return string.Empty;
        }

        private string GetFilePath(int index)
        {
            string[] filepathlist =
            {
                "pdf-succinctly.pdf",
                "hive-succinctly.pdf",
                "gis-succinctly.pdf",
                "javascript-succinctly.pdf",
                "http-succinctly.pdf"
            };

            if(index >= 1 && index <= filepathlist.Length)
            {
                return filepathlist[index - 1];
            }
            return string.Empty;
        }

        public class Data
        {
            public string FileName { get; set; }

            public string FilePath { get; set; }

            public string Author { get; set; }
        }
        #endregion

        #region AI Assist
        private Syncfusion.Blazor.InteractiveChat.SfAIAssistView aIAssistView;
        private bool isPopupVisible = false;
        private bool isMobileDevice = false;
        private bool isDocumentLoaded = false;
        private bool isSummarized = false;
        private bool isDocumentLoadedInAI = false;
        private bool refreshContainer = false;

        private MemoryStream documentStream = new MemoryStream();
        private string _promptPlaceholder = "Type your prompt for assistance...";


        private List<string> _promptSuggestions = new List<string>();

        private void PromptToolbarItemClicked(AssistViewToolbarItemClickedEventArgs args)
        {
            // handle your actions
        }

        private async Task ResponseToolbarItemClicked(AssistViewToolbarItemClickedEventArgs args)
        {
            if(args.Item.IconCss == "e-icons e-aiassist-copy")
            {
                string pattern = @"<a[^>]*>(\d+)</a>";
                string textToCopy = Regex.Replace(_prompts[args.DataIndex].Response, pattern, "$1");
                await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", textToCopy);
            }
        }

        private List<AssistViewPrompt> _prompts = new List<AssistViewPrompt>();

        private void ToggleAssistView()
        {
            isPopupVisible = !isPopupVisible;
            refreshContainer = true;
        }

        // ✨✨✨✨ Will be called when the prompt is entered ✨✨✨✨
        private async Task OnPromptRequested(AssistViewPromptRequestedEventArgs args)
        {
            await Task.Delay(2000);
            string suggestions = string.Empty;
            string response = string.Empty;


            //get the response from the AI for the prompt
            if(args.Prompt == "Summarize this document.")
            {
                response = await SummaryPDF("Summarize this document.");
            } else
            {
                var answer = await GetAnswer(args.Prompt);
                response = answer;
            }

            // Split by "suggestions"
            string[] responseArray = response.Split(new[] { "suggestions" }, StringSplitOptions.None);
            if(responseArray.Length > 1)
            {
                args.Response = responseArray[0];
                suggestions = responseArray[1];
            }


            _promptSuggestions = new List<string>(
                suggestions.Split('\n')
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s => System.Text.RegularExpressions.Regex.Replace(s, @"^\d+\.\s*", string.Empty)));
            string pattern = @"\[(?:Page )?(\d+(?:,\s*\d+)*)\]";

            //Replace the pattern with an HTML anchor tag
            args.Response = Regex.Replace(
                args.Response,
                pattern,
                m =>
                {
                    // Split the matched value by commas and optional spaces
                    var pages = m.Groups[1].Value.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var links = string.Join(
                        ", ",
                        pages.Select(page => $"<a href=\"/#\" onclick=\"goToPage({page})\">{page}</a>"));
                    return $"[{links}]";
                });
        }

        [JSInvokable]
        public async Task GoToPage(int pageNumber)
        {
            if(pageNumber > 0)
            {
                await viewer.GoToPageAsync(pageNumber);
                if(isMobileDevice)
                {
                    isPopupVisible = false;
                    StateHasChanged();
                }
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if(firstRender)
            {
                isMobileDevice = await JsRuntime.InvokeAsync<bool>("isMobileDevice", false);
                var dotNetObjectRef = DotNetObjectReference.Create(this);
                await JsRuntime.InvokeVoidAsync("initializeJSInterop", dotNetObjectRef);
            }


            await base.OnAfterRenderAsync(firstRender);
            if(refreshContainer)
            {
                await Task.Delay(300);
                await viewer.UpdateViewerContainerAsync();
                if(isPopupVisible && !isSummarized)
                {
                    isSummarized = true;
                    //Initial prompt for AI
                    await aIAssistView.ExecutePromptAsync("Summarize this document.");
                    StateHasChanged();
                }
                refreshContainer = false;
            }
        }

        private async void ToolbarItemClicked(AssistViewToolbarItemClickedEventArgs args)
        {
            if(args.Item.IconCss == "e-icons e-close")
            {
                // Close the popup and refresh the container
                isPopupVisible = false;
                refreshContainer = true;
            } else if(args.Item.IconCss == "e-icons e-refresh")
            {
                if(_prompts.Count > 0)
                {
                    AssistViewPrompt lastPrompt = _prompts.Last();
                    // Remove the last prompt from the list
                    _prompts.RemoveAt(_prompts.Count - 1);
                    // Request the last prompt from the list
                    await aIAssistView.ExecutePromptAsync(lastPrompt.Prompt);
                }
            }
        }

        public void Dispose()
        {
            isDocumentLoaded = false;
            isSummarized = false;
            isDocumentLoadedInAI = false;
            isPopupVisible = false;
            _prompts?.Clear();
            _promptSuggestions?.Clear();

            documentStream?.Dispose();
        }

        public void CleanAndOpenNewDocument()
        {
            isDocumentLoaded = false;
            isSummarized = false;
            isDocumentLoadedInAI = false;
            isPopupVisible = false;
            _prompts?.Clear();
            _promptSuggestions?.Clear();

            documentStream?.Dispose();
        }

        #region AI Query Methods
        private async Task<string> SummaryPDF(string question)
        {
            string summary = string.Empty;
            string systemPrompt = $"You are a document analyzer. Your task is to analyze the provided document {HeaderText} and generate short summary. Always respond in proper HTML format, but do not include <html>, <head>, or <body> tags.";
            if(!isDocumentLoadedInAI)
            {
                var answer = await s_memory.AskAsync(systemPrompt, minRelevance: 0.5);
                summary = answer.Result;
                var suggestions = await GetSuggestions();
                summary += "\nsuggestions" + suggestions;
                isDocumentLoadedInAI = true;
            }
            //get the summary of the PDF
            return summary;
        }

        public async Task<string> GetAnswer(string question)
        {
            question = $"You are a document analyzer. Your task is to analyze the provided document {HeaderText} pages and pick a precise page to answer the user question, proivde a reference at the bottom of the content with page numbers like ex: Reference: [20,21,23]. Pages: [pages] and using this text {question} generate a respond. Always respond in proper HTML format, but do not include <html>, <head>, or <body> tags.";

            var answer = await s_memory.AskAsync(question);

            var suggestions = await GetSuggestions();
            var result = answer.Result + "\nsuggestions" + suggestions;
            return result;
        }

        internal async Task<string> GetSuggestions()
        {
            string question = $"You are a helpful assistant. Your task is to analyze the provided document {HeaderText} and generate 3 short diverse questions and each question should not exceed 10 words";
            var answer = await s_memory.AskAsync(question, minRelevance: 0.5);
            return answer.Result;
        }
        #endregion

        #endregion

        #region Document Store Management

        // Simple file upload, with document ID
        private static async Task StoreFile(string docName, string documentId)
        {
            try
            {
                if(!await s_memory.IsDocumentReadyAsync(documentId: documentId))
                {
                    var docId = await s_memory.ImportDocumentAsync(docName, documentId: documentId);
                    s_toDelete.Add(docId);
                }
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion
    }
}
