using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Taskbar_Plus_Plus.Helpers
{
    [ComImport]
    [Guid("00021401-0000-0000-C000-000000000046")]
    internal class ShellLink
    {
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214F9-0000-0000-C000-000000000046")]
    internal interface IShellLinkW
    {
        void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, IntPtr pfd,
            int fFlags);

        void GetIDList(out IntPtr ppidl);
        void SetIDList(IntPtr pidl);
        void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
        void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
        void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
        void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
        void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
        void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
        void GetHotkey(out short pwHotkey);
        void SetHotkey(short wHotkey);
        void GetShowCmd(out int piShowCmd);
        void SetShowCmd(int iShowCmd);

        void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath,
            out int piIcon);

        void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
        void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
        void Resolve(IntPtr hwnd, int fFlags);
        void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
    }

    [ComImport]
    [Guid("0000010B-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPersistFile
    {
        void GetClassID(out Guid pClassID);
        void IsDirty();
        void Load([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, int dwMode);
        void Save([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, bool fRemember);
        void SaveCompleted([MarshalAs(UnmanagedType.LPWStr)] string pszFileName);
        void GetCurFile([MarshalAs(UnmanagedType.LPWStr)] out string ppszFileName);
    }

    public static class ShortcutHelper
    {
        public static void CreateShortcut(string shortcutPath, string targetPath, string iconPath)
        {
            IShellLinkW link = (IShellLinkW)new ShellLink();
            link.SetDescription(Path.GetFileNameWithoutExtension(shortcutPath));
            link.SetPath(App.TPP_EXE_PATH);
            link.SetArguments($"\"{targetPath}\"");
            link.SetIconLocation(iconPath, 0);

            IPersistFile file = (IPersistFile)link;
            file.Save(shortcutPath, false);
        }
        
        /// <summary>
        /// Pin the shortcut to the taskbar.
        /// This method uses PowerShell to pin the shortcut to the taskbar.
        /// ! NOT WORKING !
        /// </summary>
        /// <param name="shortcutPath"></param>
        public static void PinToTaskbar(string shortcutPath)
        {
            // Note: The following code is commented out because it does not work as expected.
            // Process.Start(new ProcessStartInfo() 
            // {
            //     FileName = "powershell",
            //     Arguments = $"-Command \"& {{ (New-Object -ComObject Shell.Application).Namespace((Split-Path \"${shortcutPath}\")).ParseName((Split-Path \"${shortcutPath}\" -Leaf)).Verbs() | Where-Object {{ $_.Name -match 'taskbar|barre des tâches' }} | ForEach-Object {{ $_.DoIt() }}\n }}\"",
            //     CreateNoWindow = true,
            //     UseShellExecute = false,
            //     RedirectStandardOutput = true,
            //     RedirectStandardError = true
            // });
        }
    }
}