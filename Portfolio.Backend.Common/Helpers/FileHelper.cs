using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Portfolio.Backend.Common.Helpers
{
    public static class FileHelper
    {

        public static string CreateFolder(string path, string name)
        {
            Console.WriteLine("Create Folder: {0} {1}", path, name);
            if (string.IsNullOrEmpty(path)) throw new Exception("Need to provide a directory path to this type of file");
            if (!Directory.Exists(path)) throw new Exception("Given directory does not exist");
            path = Path.Combine(path, name);
            if (Directory.Exists(path)) return path;
            Directory.CreateDirectory(path);
            return path;
        }

        public static void DeleteFolder(string path)
        {
            Directory.Delete(path);
        }

        public static string CreateFile(string path, IFormFile file)
        {
            if (string.IsNullOrEmpty(path)) throw new Exception("Need to provide a directory path to this type of file");
            if (!Directory.Exists(path)) throw new Exception("Given directory does not exist");
            path = Path.Combine(path, file.FileName);
            var fs = File.Create(path);
            file.CopyTo(fs);
            fs.Close();
            return path;
        }

        public static string CreateFileAndFolder(string path, IFormFile file)
        {
            Console.WriteLine("path:{0}", path);
            Console.WriteLine("filename:{0}", file.FileName);
            if (string.IsNullOrEmpty(path)) throw new Exception("Need to provide a directory path to this type of file");
            if (!Directory.Exists(path))
            {
                var pathParts = path.Split("/");
                pathParts[0] = "/";
                Console.WriteLine("PathParts:");
                for (int i = 0; i < pathParts.Length; i++)
                {
                    Console.WriteLine(pathParts[i]);
                }

                // / Users / sckmk / OnlyMac / Portfolio / Images / Ideas
                for (int i = 1; i < pathParts.Length; i++)
                {
                    string last = pathParts[i];
                    string first = string.Join("/", pathParts, 0, i);
                    Console.WriteLine(first);
                    Console.WriteLine(last);
                    CreateFolder(first, last);
                }

            }
            path = Path.Combine(path, file.FileName);
            var fs = File.Create(path);
            file.CopyTo(fs);
            fs.Close();
            return path;
        }

        public static void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public static string[] CreateFileBatch(List<Tuple<string, IFormFile>> fileList)
        {
            List<string> results = new();
            foreach (var fileTuple in fileList)
            {
                results.Add(CreateFile(fileTuple.Item1, fileTuple.Item2));
            }
            return results.ToArray();
        }

        public static void DeleteFileBatch(string[] paths)
        {
            foreach (var path in paths)
            {
                DeleteFile(path);
            }
        }
    }
}

