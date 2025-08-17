using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using FolderBrowserEx;
using Microsoft.Win32;
using Taskbar_Plus_Plus.Helpers;

namespace Taskbar_Plus_Plus.Windows
{
    public partial class TaskbarPlusPlus : Window
    {
        public TaskbarPlusPlus()
        {
            InitializeComponent();
        }

        private void BrowseLinkedFolder(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog()
            {
                Title = "Select the linked folder",
                InitialFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                AllowMultiSelect = false,
            };
            
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LinkedFolderInput.Text = folderBrowserDialog.SelectedFolder;
            }
        }

        private void BrowseShortcutIcon(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Title = "Choisir l'icône",
                Filter = "Icônes (*.ico;*.exe;*.dll)|*.ico;*.exe;*.dll|Tous les fichiers (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                Multiselect = false,
                CheckFileExists = true,
            };
            if (ofd.ShowDialog() == true)
                IconPathInput.Text = ofd.FileName;
        }

        private void CreateShortcut_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                string name = TxtName.Text.Trim();
                string linkedFolder = LinkedFolderInput.Text.Trim();
                string icon = IconPathInput.Text.Trim();
                
                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Please specify a name for the shortcut.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(linkedFolder))
                {
                    MessageBox.Show("Please specify a linked folder.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!Directory.Exists(linkedFolder))
                {
                    MessageBox.Show("The specified linked folder does not exist. ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(icon) || !File.Exists(icon))
                {
                    MessageBox.Show("The specified icon does not exist. ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                
                string lnkPath = Path.Combine(App.TPP_SHORTCUTS_FOLDER_PATH, $"{SanitizeFileName(name)}.lnk");

                Console.Out.WriteLine(lnkPath);
            
                ShortcutHelper.CreateShortcut(lnkPath, linkedFolder, icon);
                ShortcutHelper.PinToTaskbar(lnkPath); // Pinning to taskbar is not working as of now
                
                MessageBoxResult result = MessageBox.Show($"Shortcut created successfully: {Path.GetFileName(lnkPath)}\nYou can pin it to the task bar !", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                
                if (result == MessageBoxResult.OK)
                {
                    // As we cannot pin the shortcut to the taskbar directly, we will open the parent directory
                    // Open the parent directory of the shortcut
                    Process.Start(new ProcessStartInfo()
                    {
                        FileName = App.TPP_SHORTCUTS_FOLDER_PATH,
                        UseShellExecute = true,
                        Verb = "open"
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot create shortcut:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static string SanitizeFileName(string name)
        {
            return Path.GetInvalidFileNameChars().Aggregate(name, (current, c) => current.Replace(c, '_'));
        }
    }
}