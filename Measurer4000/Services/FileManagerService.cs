using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Measurer4000.Core.Services.Interfaces;
using Measurer4000.Core.Utils;

namespace Measurer4000.Services
{
    public class FileManagerService : IFileManagerService
    {
        public List<string> DirectorySearch(string directory, string searchPattern = "")
        {
            string[] searchPatterns = searchPattern.Split('|');

            var files = new List<string>();
            var directories = ListingDirectories(directory);

            var directoriesToRemove = directories.Where(x => x.Contains("\\bin\\") || x.Contains("\\obj\\")).ToList();

            foreach(var dirRemove in directoriesToRemove)
            {
                directories.Remove(dirRemove);
            }
            
            foreach(var ptrn in searchPatterns)
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

            foreach(var subDirectory in Directory.GetDirectories(path))
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
