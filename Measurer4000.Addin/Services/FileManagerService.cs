using System.Collections.Generic;
using System.IO;
using System.Linq;
using Measurer4000.Core.Services.Interfaces;

namespace Measurer4000.Addin.Services
{
    class FileManagerService : IFileManagerService
    {
        public List<string> DirectorySearch(string directory, string searchPattern = "")
        {
            string[] searchPatterns = searchPattern.Split('|');

            var files = new List<string>();
            var directories = ListingDirectories(directory);

            var directoriesToRemove = directories.Where(x => x.Contains("\\bin\\") || x.Contains("\\obj\\")).ToList();

            foreach (var dirRemove in directoriesToRemove)
            {
                directories.Remove(dirRemove);
            }

            foreach (var ptrn in searchPatterns)
            {
                foreach (var subDirectory in directories)
                {
                    files.AddRange(Directory.GetFiles(subDirectory, ptrn));
                }
            }

            return files;
        }

        private List<string> ListingDirectories(string path)
        {
            var directories = new List<string>();

            foreach (var subDirectory in Directory.GetDirectories(path))
            {
                directories.Add(subDirectory);
                directories.AddRange(ListingDirectories(subDirectory));
            }

            return directories;
        }

        public Stream OpenRead(string filePath)
        {
            return File.OpenRead(filePath);
        }
    }
}