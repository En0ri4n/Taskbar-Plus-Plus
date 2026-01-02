# Taskbar++ (Taskbar Plus Plus)

A Windows desktop utility that enhances your taskbar experience by allowing you to create custom shortcuts that open popup toolbars with quick access to folders and applications.

## 🌟 Features

- **Custom Taskbar Shortcuts**: Create shortcuts that can be pinned to your Windows taskbar
- **Popup Toolbars**: Click on a shortcut to open a sleek popup toolbar displaying all files and applications in a linked folder
- **Custom Icons**: Set custom icons for your shortcuts (supports .ico, .exe, and .dll files)
- **Auto-Positioning**: Popups automatically position themselves above the taskbar with proper alignment
- **Quick Access**: Launch applications and files directly from the popup toolbar
- **Auto-Close**: Popup automatically closes when it loses focus, behaving like native Windows context menus

## 📋 Requirements

- Windows OS (Windows 10 or later recommended)
- .NET Core 3.1 Runtime
- WPF support

## 🚀 Installation

1. Download the latest release of `TaskbarPlusPlus.exe`
2. Run the executable - it will automatically:
   - Create a folder at `%APPDATA%\TaskbarPlusPlus\`
   - Install itself to that location
   - Create a `TPP-Shortcuts` folder for your shortcuts

## 💡 Usage

### Creating a Shortcut

1. Launch `TaskbarPlusPlus.exe`
2. The **Taskbar++ Shortcut Creator** window will appear
3. Fill in the following fields:
   - **Shortcut name**: Enter a name for your shortcut
   - **Linked folder**: Browse and select the folder containing the applications/files you want quick access to
   - **Shortcut icon**: Browse and select an icon file (.ico, .exe, or .dll)
4. Click **Create shortcut**
5. The shortcut will be created in the `TPP-Shortcuts` folder
6. A file explorer window will open - right-click the shortcut and pin it to your taskbar

### Using a Shortcut

1. Click on your pinned Taskbar++ shortcut
2. A popup toolbar will appear above your taskbar showing all files from the linked folder
3. Click on any icon to launch the corresponding application or file
4. The popup automatically closes when you click outside it or launch an application

## 🔧 How It Works

Taskbar++ uses a clever architecture to enhance your Windows taskbar:

1. **Shortcut Creation**: Creates Windows shortcuts (.lnk files) that point to the Taskbar++ executable with the target folder as an argument
2. **Popup Display**: When a shortcut is clicked, the application launches in popup mode, reading all files from the specified folder
3. **Dynamic Layout**: The popup window automatically sizes and positions itself based on:
   - Number of files in the folder
   - Taskbar position and height
   - Screen dimensions
4. **Icon Extraction**: Uses native Windows APIs to extract and display proper icons for each file type

## 📁 Project Structure

```
Taskbar-Plus-Plus/
├── App.xaml                    # Application definition
├── App.xaml.cs                 # Application startup logic
├── Windows/
│   ├── TaskbarPlusPlus.xaml    # Main shortcut creator window
│   ├── TaskbarPlusPlus.xaml.cs # Shortcut creation logic
│   ├── ToolsPopup.xaml         # Popup toolbar window
│   └── ToolsPopup.xaml.cs      # Popup display logic
├── Helpers/
│   ├── FileVisualHelper.cs     # Icon extraction utilities
│   ├── IconExtractor.cs        # Windows icon extraction APIs
│   ├── ShortcutHelper.cs       # Windows shortcut creation
│   └── TaskbarHelper.cs        # Taskbar position detection
└── icon.ico                    # Application icon
```

## 🛠️ Building from Source

### Prerequisites

- .NET Core 3.1 SDK
- Visual Studio 2019 or later (or JetBrains Rider)
- Windows OS

### Build Steps

1. Clone the repository:
   ```bash
   git clone https://github.com/En0ri4n/Taskbar-Plus-Plus.git
   cd Taskbar-Plus-Plus
   ```

2. Open the solution:
   ```bash
   Taskbar-Plus-Plus.sln
   ```

3. Build the project:
   ```bash
   dotnet build
   ```

4. Run the application:
   ```bash
   dotnet run
   ```

## 📝 Technical Details

- **Framework**: .NET Core 3.1 with WPF
- **UI**: Windows Presentation Foundation (WPF)
- **Dependencies**:
  - FolderBrowserEx (v1.0.1) - Enhanced folder browser dialog

## ⚠️ Known Limitations

- Automatic pinning to taskbar via code is not currently supported by Windows - users must manually pin shortcuts
- The popup toolbar tooltip horizontal centering is currently not working as expected

## 🤝 Contributing

Contributions are welcome! Feel free to:

- Report bugs
- Suggest new features
- Submit pull requests

## 📄 License

This project is open source. Please check the repository for license details.

## 👨‍💻 Author

Created by [En0ri4n](https://github.com/En0ri4n)

---

**Note**: This application is designed for Windows and uses Windows-specific APIs for taskbar integration and icon extraction.
