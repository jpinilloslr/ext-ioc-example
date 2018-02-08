using System.Collections.Generic;
using System.IO;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Wrappers
{
    public static class FileSystem
    {
        public static string ReadFileText(string filename)
        {
            return File.ReadAllText(filename);
        }

        public static IEnumerable<string> GetDirectoryFiles(string path, string searchPattern)
        {
            return Directory.EnumerateFiles(path, searchPattern);
        }

        public static string CombinePath(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }

        public static string GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        public static string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }
    }
}