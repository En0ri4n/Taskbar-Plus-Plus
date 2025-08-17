using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Taskbar_Plus_Plus.Helpers
{
    public static class IconExtractor
    {
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SHGetFileInfo(
            string pszPath,
            uint dwFileAttributes,
            out SHFILEINFO psfi,
            uint cbFileInfo,
            uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool DestroyIcon(IntPtr hIcon);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        private const uint SHGFI_ICON = 0x000000100;
        private const uint SHGFI_LARGEICON = 0x000000000;
        private const uint SHGFI_SMALLICON = 0x000000001;
        private const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;
        private const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;

        // Récupère l’icône associée par le Shell (fichier existant ou extension, ex: ".ahk").
        public static ImageSource ExtractIconFromFileOrAssociatedFile(string pathOrExt, bool large = true)
        {
            string input = pathOrExt;
            uint flags = SHGFI_ICON | (large ? SHGFI_LARGEICON : SHGFI_SMALLICON);
            uint attrs = 0;

            bool exists;
            try
            {
                exists = File.Exists(pathOrExt) || Directory.Exists(pathOrExt);
            }
            catch { exists = false; }

            if (!exists)
            {
                // Use the extension to find the associated icon.
                string ext = pathOrExt;
                if (!ext.StartsWith(".", StringComparison.Ordinal))
                    ext = Path.GetExtension(pathOrExt);
                if (string.IsNullOrEmpty(ext)) return null;

                input = ext.StartsWith(".") ? ext : "." + ext;
                flags |= SHGFI_USEFILEATTRIBUTES;
                attrs = FILE_ATTRIBUTE_NORMAL;
            }

            if (SHGetFileInfo(input, attrs, out var sfi, (uint)Marshal.SizeOf<SHFILEINFO>(), flags) == IntPtr.Zero ||
                sfi.hIcon == IntPtr.Zero)
                return null;

            try
            {
                BitmapSource img = Imaging.CreateBitmapSourceFromHIcon(
                    sfi.hIcon, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                img.Freeze();
                return img;
            }
            finally
            {
                DestroyIcon(sfi.hIcon);
            }
        }

        public static ImageSource LoadFromImageFile(string filePath)
        {
            using FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = fs;
            bitmap.EndInit();
            bitmap.Freeze();
            return bitmap;
        }
    }
}