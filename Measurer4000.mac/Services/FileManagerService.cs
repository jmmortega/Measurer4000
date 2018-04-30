using System.IO;
using Measurer4000.Core.Services.Interfaces;
using Foundation;
using System.Collections.Generic;
using System;

namespace Measurer4000.mac.Services
{
    public class FileManagerService : IFileManagerService
    {        
        public Stream OpenRead(string filePath)
        {
			var path = filePath.Replace("\\", "/");
            return NSFileHandle.OpenRead(path).ReadDataToEndOfFile().AsStream();
        }

        public List<string> DirectorySearch(string directory)
        {
            var files = new List<string>();
            try
            {
                foreach(var subDirectory in Directory.GetDirectories(directory))
                {
                    foreach(var file in Directory.GetFiles(subDirectory))
                    {
                        files.Add(file);
                    }
                    files.AddRange(DirectorySearch(subDirectory));
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return files;
        }
        
    }
}
