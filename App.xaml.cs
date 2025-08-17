using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Taskbar_Plus_Plus.Windows;

namespace Taskbar_Plus_Plus
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly string TPP_EXE_NAME = "TaskbarPlusPlus.exe";
        public static readonly string TPP_FOLDER_NAME = "TaskbarPlusPlus";
        public static readonly string TPP_SHORTCUTS_FOLDER_NAME = "TPP-Shortcuts";

        public static readonly string TPP_FOLDER_PATH =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), TPP_FOLDER_NAME);

        public static readonly string TPP_SHORTCUTS_FOLDER_PATH =
            Path.Combine(TPP_FOLDER_PATH, TPP_SHORTCUTS_FOLDER_NAME);
        
        public static readonly string TPP_EXE_PATH = Path.Combine(TPP_FOLDER_PATH, TPP_EXE_NAME);

        protected override void OnStartup(StartupEventArgs e)
        {
            CheckDirectories();
            
            InstallExecutable();

            // Path to your current executable
            string sourcePath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            // Destination path (for example, on Desktop)
            string destPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "TaskbarPlusPlus.exe");

            // Copy the executable
            File.Copy(sourcePath, destPath, overwrite: true);


            if (e.Args.Length > 0)
            {
                string folderPath = e.Args[0];

                ToolsPopup toolsPopup = new ToolsPopup(folderPath);
                toolsPopup.Closed += (_, __) => Shutdown();
                toolsPopup.Show();
            }
            else
            {
                TaskbarPlusPlus mainWindow = new TaskbarPlusPlus();
                mainWindow.Show();
            }

            base.OnStartup(e);
        }

        private static void CheckDirectories()
        {
            if (!Directory.Exists(TPP_FOLDER_PATH))
                Directory.CreateDirectory(TPP_FOLDER_PATH);

            if (!Directory.Exists(TPP_SHORTCUTS_FOLDER_PATH))
                Directory.CreateDirectory(TPP_SHORTCUTS_FOLDER_PATH);
        }

        private static void InstallExecutable()
        {
            string sourcePath = Process.GetCurrentProcess().MainModule?.FileName;
            string destPath = TPP_EXE_PATH;

            if (File.Exists(destPath)) return;
            
            try
            {
                Console.Out.WriteLine(sourcePath);
                Console.Out.WriteLine(destPath);
                File.Copy(sourcePath, destPath, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to install the executable: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}