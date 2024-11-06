# Home Component Documentation

## Overview
The `Home` component is a Blazor page that integrates a PDF viewer and AI-assisted features for document interaction. It leverages Syncfusion's Blazor components to provide a robust document management system.

---

## Home.razor

### Overview
`Home.razor` is the Razor file responsible for defining the UI and layout of the `Home` component.

### Key Features
- Displays a PDF viewer.
- Provides an AI-assisted prompt interface.
- Supports viewing and annotating PDFs.

### Razor Markup
```razor
<Syncfusion.Blazor.InteractiveChat.SfAIAssistView>
    <!-- Add additional UI elements here -->
</Syncfusion.Blazor.InteractiveChat.SfAIAssistView>

<SfPdfViewer2 @ref="viewer"
              DocumentPath="@DocumentPath"
              ToolbarSettings="@ToolbarSettings"
              ContextMenuSettings="@ContextMenuSettings"
              AnnotationSettings="@annotationSettings"
              EnableTextSelection="@enableTextSelection"
              EnableStickyNotesAnnotation="@enableStickyNotesAnnotation"
              IsReadOnlyMode="@isReadOnlyMode"
              OnDocumentLoaded="DocumentLoaded">
</SfPdfViewer2>

<!-- Additional UI elements -->
```

### CSS Styling
```html
<style>
    /* Custom styling for the Home component */
</style>
```

---

## Home.razor.cs

### Overview
`Home.razor.cs` contains the logic for handling PDF viewer interactions, AI-assisted document analysis, and more.

### Fields and Properties
- **`memoryService`**: Handles AI memory operations.
- **`s_memory`**: Represents an instance of memory for AI interactions.
- **`DataItem`**: List of documents represented as `Data` objects.

### Methods

#### `DocumentLoaded(LoadEventArgs args)`
**Purpose**: Handles the event when a document is loaded.
- **Parameters**: `args` - Provides information about the loaded document.
- **Functionality**:
  - Resets state variables.
  - Configures read-only settings if necessary.
  - Calls `StoreFile` to save the document in the memory.

#### `CommandClick(CommandClickEventArgs<Data> args)`
**Purpose**: Handles toolbar command clicks (view/edit PDF).
- **Parameters**: `args` - Contains data about the clicked command.
- **Functionality**:
  - Opens a dialog and configures viewer settings based on the command.

#### `PromptToolbarItemClicked(AssistViewToolbarItemClickedEventArgs args)`
**Purpose**: Handles toolbar item clicks in the AI Assist view.
- **Parameters**: `args` - Contains data about the toolbar item clicked.

#### `OnPromptRequested(AssistViewPromptRequestedEventArgs args)`
**Purpose**: Processes AI prompts entered by the user.
- **Parameters**: `args` - Provides the prompt and collects the AI response.
- **Functionality**:
  - Sends prompts to the AI and processes responses.

#### `GetAnswer(string question)`
**Purpose**: Fetches AI-generated responses for user queries.
- **Parameters**: `question` - The user’s question or prompt.

#### `SummaryPDF(string question)`
**Purpose**: Summarizes the loaded PDF document using AI.
- **Parameters**: `question` - The summary request prompt.

#### `StoreFile(string docName, string documentId)`
**Purpose**: Stores the document in the memory service.
- **Parameters**:
  - `docName`: Path to the document.
  - `documentId`: Unique identifier for the document.

#### `GoToPage(int pageNumber)`
**Purpose**: Navigates to a specified page in the PDF.
- **Parameters**: `pageNumber` - The page number to navigate to.

#### `GetSuggestions()`
**Purpose**: Fetches AI-generated suggestions for document questions.

### Lifecycle Methods

#### `OnInitialized()`
**Purpose**: Initializes the component by loading sample data.

#### `OnAfterRenderAsync(bool firstRender)`
**Purpose**: Handles operations after the component is rendered.
- **Parameters**: `firstRender` - Indicates if this is the first render.

### Helper Methods

#### `GetFileName(int index)`
**Purpose**: Returns the file name for a document.
- **Parameters**: `index` - The index of the document.

#### `GetAuthor(int index)`
**Purpose**: Returns the author of a document.
- **Parameters**: `index` - The index of the document.

#### `GetFilePath(int index)`
**Purpose**: Returns the file path for a document.
- **Parameters**: `index` - The index of the document.

### Cleanup and Disposal

#### `Dispose()`
**Purpose**: Cleans up resources when the component is disposed.

#### `CleanAndOpenNewDocument()`
**Purpose**: Resets the component state and prepares for a new document.

### Inner Classes

#### `Data`
Represents metadata for documents.
- **Properties**:
  - `FileName`: Name of the document.
  - `FilePath`: Path of the document.
  - `Author`: Author of the document.

---

## Notes
- Ensure Syncfusion and Azure services are properly configured for full functionality.
- The AI features depend on backend services for processing prompts and documents.
