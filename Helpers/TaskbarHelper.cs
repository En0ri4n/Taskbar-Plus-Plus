using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace Taskbar_Plus_Plus.Helpers
{
    public static class TaskbarHelper
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, out RectBox lpRectBox);

        [StructLayout(LayoutKind.Sequential)]
        public struct RectBox
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
            
            public int Width => Right - Left;
            public int Height => Bottom - Top;

            public override string ToString()
            {
                return $"Left: {Left}, Top: {Top}, Right: {Right}, Bottom: {Bottom}";
            }
        }

        public static RectBox GetTaskbarPosition()
        {
            IntPtr taskbarHandle = FindWindow("Shell_TrayWnd", null);
            GetWindowRect(taskbarHandle, out RectBox rect);
            return rect;
        }
    }

}