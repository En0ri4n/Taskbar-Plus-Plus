using System.IO;
using System.Linq;
using System.Windows.Media;

namespace Taskbar_Plus_Plus.Helpers
{
    public static class FileVisualHelper
    {
        private static readonly string[] ImageExtensions = { ".png", ".jpg", ".jpeg", ".bmp", ".gif" };
        public static ImageSource GetVisual(string filePath)
        {
            string ext = Path.GetExtension(filePath).ToLower();
            
            return !ImageExtensions.Contains(ext) ? IconExtractor.ExtractIconFromFileOrAssociatedFile(filePath) : IconExtractor.LoadFromImageFile(filePath);
        }
    }

}