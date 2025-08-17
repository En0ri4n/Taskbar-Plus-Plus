using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;
using Taskbar_Plus_Plus.Helpers;

namespace Taskbar_Plus_Plus.Windows
{
    public partial class ToolsPopup
    {
        public ObservableCollection<Button> AppButtons { get; } = new ObservableCollection<Button>();
        private bool _autoCloseEnabled;
        
        private readonly string _folderPath;
        private string[] _files;
        private double _iconSize;

        public ToolsPopup(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show($"The specified folder does not exist: {folderPath}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                throw new DirectoryNotFoundException($"The specified folder does not exist: {folderPath}");
            }
            
            _folderPath = folderPath ?? throw new ArgumentNullException(nameof(folderPath));
            
            InitializeComponent();
            
            InitializeClosingBehavior();

            FindFiles();
            
            SetupWindowProperties();

            foreach (string filePath in _files)
            {
                ImageSource icon = FileVisualHelper.GetVisual(filePath);

                ToolTip tooltip = new ToolTip
                {
                    Content = Path.GetFileNameWithoutExtension(filePath),
                    Style = Resources["AppIconToolTip"] as Style,
                    Placement = PlacementMode.Top
                };

                Button button = new Button
                {
                    Content = new Image { Source = icon },
                    Width = _iconSize,
                    Height = _iconSize,
                    ToolTip = tooltip,
                    Template = Resources["AppIcon"] as ControlTemplate
                };

                tooltip.PlacementTarget = button;
                // Center the tooltip horizontally (does not work as of now)
                // tooltip.HorizontalOffset = (iconSize - tooltip.ActualWidth) / 2 - tooltip.ActualWidth / 2;

                button.Click += (s, e) => StartApplication(filePath);

                AppButtons.Add(button);
            }
        }

        private void FindFiles()
        {
            _files = Directory.GetFiles(_folderPath, "*.*");
        }

        /// <summary>
        /// Initializes the closing behavior of the popup window.
        /// The window will close automatically when it loses focus.
        /// It also sets the window to be topmost and activates it on load.
        /// This is useful for creating a popup that behaves like a taskbar context menu.
        /// </summary>
        private void InitializeClosingBehavior()
        {
            Topmost = true;
            Loaded += (_, __) =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    _autoCloseEnabled = true;
                    Activate();
                }), DispatcherPriority.ApplicationIdle);
            };

            Deactivated += (_, __) =>
            {
                if (_autoCloseEnabled)
                    ExitApplication();
            };
        }

        /// <summary>
        /// Set up the window properties based on the taskbar position and icon size.
        /// </summary>
        private void SetupWindowProperties()
        {
            TaskbarHelper.RectBox taskbarRectBox = TaskbarHelper.GetTaskbarPosition();

            _iconSize = Math.Min(90, taskbarRectBox.Height * 0.85);
            
            Height = taskbarRectBox.Height + 10;
            Width = _iconSize * _files.Length + 30 * Math.Max(0, _files.Length - 1);
            Top = taskbarRectBox.Top - Height;
            Left = taskbarRectBox.Width / 2D - Width / 2D;
        }

        /// <summary>
        /// Starts the application specified by the given executable path.
        /// The method uses `cmd.exe` to start the application in a new window, so it will be launch as separate process.
        /// If the application fails to start, an error message is displayed to the user.
        /// </summary>
        /// <param name="filePath">The path to the file to start.</param>
        private static void StartApplication(string filePath)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c start \"\" \"{filePath}\"",
                    CreateNoWindow = true,
                    UseShellExecute = false
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to start application: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Closes the popup window.
        /// This method is called when the popup loses focus or when the user decides to close it.
        /// </summary>
        private void ExitApplication()
        {
            Close();
        }
    }
}