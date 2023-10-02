using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Portfolio.Backend.Common.Helpers
{
	public static class FileHelper
	{

		public static string CreateFolder(string path, string name)
		{
			if (string.IsNullOrEmpty(path)) throw new Exception("Need to provide a directory path to this type of file");
            if (!Directory.Exists(path)) throw new Exception("Given directory does not exist");
            path = Path.Combine(path, name);
			if (Directory.Exists(path)) throw new Exception("Directory already exists");
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
            return path;
        }

		public static string CreateFileAndFolder(string path, IFormFile file)
        {
            if (string.IsNullOrEmpty(path)) throw new Exception("Need to provide a directory path to this type of file");
            if (!Directory.Exists(path))
            {
                var pathParts = path.Split("/");

                // / Users / sckmk / OnlyMac / Portfolio / Images / Ideas
                string last = pathParts.Last();
                string first = string.Join("/", pathParts, 0, pathParts.Length - 1);
                CreateFolder(first, last);
            }
            path = Path.Combine(path, file.FileName);
            var fs = File.Create(path);
            file.CopyTo(fs);
            return path;
        }

        public static void DeleteFile(string path)
		{
			File.Delete(path);
		}

		public static string[] CreateFileBatch(List<Tuple<string,IFormFile>> fileList)
		{
			List<string> results = new();
			foreach(var fileTuple in fileList)
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

