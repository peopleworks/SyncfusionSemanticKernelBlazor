# Syncfusion Semantic Kernel Blazor

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen)](#)

Integrate AI-assisted document interaction into your Blazor applications! This project combines a powerful PDF viewer with advanced AI features to provide a robust and interactive document management system. Leveraging **Syncfusion's Blazor components**, **Semantic Kernel**, and **Kernel Memory**, it offers a seamless and extensible experience.

## Table of Contents

- [About the Project](#about-the-project)
- [Features](#features)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)
- [Screenshots](#screenshots)
- [Roadmap](#roadmap)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)
- [Acknowledgments](#acknowledgments)
- [FAQ](#faq)

---

## About the Project

This Blazor project integrates a PDF viewer with AI-assisted features for enhanced document interaction. By leveraging **Syncfusion's Blazor components**, **Semantic Kernel**, and **Kernel Memory**, it provides a powerful and extensible document management system.

## Features

- 📚 **AI-Assisted PDF Viewing**: Interact with PDF documents enhanced by AI capabilities like search, summarization, and content analysis.
- 🎨 **Stunning UI Components**: Utilize Syncfusion's powerful Blazor controls to create breathtaking user interfaces.
- ⚡ **Unified Semantic Kernel Integration**: Use a single kernel setup to integrate multiple Semantic Kernel services seamlessly.
- 🔌 **Extensibility**: Easily expand the application to support additional providers and functionalities.
- 🚀 **High Performance**: Optimized for speed and efficiency, ensuring a smooth user experience.

## Getting Started

### Prerequisites

- **Development Environment**: Visual Studio 2019+ or Visual Studio Code
- **.NET SDK**: .NET Core 3.1 or later
- **API Keys**: Obtain API keys for the Semantic Kernel services you wish to use
- **Syncfusion License Key**: Required for Syncfusion components (free community licenses are available)

### Installation

1. **Clone the Repository**
   ```bash
   git clone https://github.com/yourusername/syncfusion-semantic-kernel-blazor.git
   ```
2. **Open the Project**
   - Navigate to the project directory and open it in Visual Studio or Visual Studio Code.
3. **Update API Keys**
   - Locate the `appsettings.json` file.
   - Update the `ApiKeys` section with your respective API keys.
4. **Register Syncfusion License**
   - Add your Syncfusion license key in the `Program.cs` file:
     ```csharp
     Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("YOUR LICENSE KEY");
     ```
5. **Install Dependencies**
   - Restore NuGet packages via your IDE or using the command line:
     ```bash
     dotnet restore
     ```
6. **Run the Application**
   - Use your IDE's build and run feature or execute:
     ```bash
     dotnet run
     ```

## Usage

- **View and Interact with PDF Documents**
  - Upload or open PDF files using the integrated Syncfusion PDF viewer.
- **Leverage AI Features**
  - Perform intelligent searches, receive document summaries, and interact with content using AI capabilities.
- **Switch Semantic Kernel Providers**
  - Easily change the AI service provider by updating the configuration in `appsettings.json`.
- **Customize and Extend**
  - Modify the UI and add new features using Syncfusion components and the extensible architecture.

## Screenshots

![2024-11-05_00-37-59](https://github.com/user-attachments/assets/3705b454-a7d0-45a3-a1a0-33698d87bb6c)


## Roadmap

- **Upcoming Features**
  - 🗄️ **RAG Integration**: Adding Retrieval-Augmented Generation (RAG) using a SQL Server Database.
  - 🛠️ **More AI Functions**: Implementing additional AI functionalities like language translation and sentiment analysis.
- **Long-Term Goals**
  - 🌐 **Expand Provider Support**: Integrate more Semantic Kernel providers and services.
  - 🎯 **Enhanced UI/UX**: Incorporate more Syncfusion components for richer user experiences.

Stay tuned for exciting updates!

## Contributing

Contributions are what make the open-source community such an amazing place to learn, inspire, and create. **Any contributions you make are greatly appreciated**.

1. **Fork the Project**
2. **Create your Feature Branch** (`git checkout -b feature/AmazingFeature`)
3. **Commit your Changes** (`git commit -m 'Add some AmazingFeature'`)
4. **Push to the Branch** (`git push origin feature/AmazingFeature`)
5. **Open a Pull Request**

## License

Distributed under the **MIT License**. See `LICENSE` file for more information.

## Contact

- **Email**: [peopleworks@gmail.com](mailto:peopleworks@gmail.com)

## Acknowledgments

- 💙 **Microsoft® Semantic Kernel Team**: For their outstanding work.
- ⭐ **Syncfusion**: For providing the best components in the .NET ecosystem.
- 🛠️ **Ollama**: For their great tools.
- 🚀 **Google**: For their fast and well-crafted models.
- 🤝 **Groq**: For being a part of the developer community.
- 🌐 **Meta**: For supporting open-source initiatives.

## FAQ

### ❓ Can I use other Semantic Kernel providers?

Yes! The architecture is designed to be extensible. You can add or switch providers by updating the configuration and implementing the necessary interfaces.

### ❓ Do I need a Syncfusion license?

Syncfusion offers a free community license if you qualify; otherwise, you may need to purchase a license for their components. Check their [licensing page](https://www.syncfusion.com/sales/communitylicense) for more information.

### ❓ How can I contribute?

See the [Contributing](#contributing) section above for details on how to get started.

---

*This README aims to provide a comprehensive overview of the Syncfusion Semantic Kernel Blazor project, assisting developers and contributors in harnessing the power of AI in Blazor applications with stunning UI components.*

---
