using System.IO;

namespace MyApp.Framework.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IOExtensions
    {
        public static bool IsFileLocked(this FileInfo file)
        {
            if (file == null)
                return false;

            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                stream?.Close();
            }
            
            return false;
        }
    }
}