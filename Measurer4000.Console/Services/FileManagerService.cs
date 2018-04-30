using Measurer4000.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace Measurer4000.Command.Services
{
    public class FileManagerService : IFileManagerService
    {
        public Stream OpenRead(string filePath)
        {
            return File.OpenRead(filePath);
        }

        public List<string> DirectorySearch(string directory)
        {
            var files = new List<string>();
            try
            {
                foreach (var subDirectory in Directory.GetDirectories(directory))
                {
                    foreach (var file in Directory.GetFiles(subDirectory))
                    {
                        files.Add(file);
                    }
                    files.AddRange(DirectorySearch(subDirectory));
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return files;
        }
    }
}
