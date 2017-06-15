using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Measurer4000.Core.Services.Interfaces;
using Foundation;

namespace Measurer4000.mac.Services
{
    class FileManagerService : IFileManagerService
    {
        public Stream OpenRead(string filePath)
        {
            
            var t = NSFileHandle.OpenRead(filePath.Replace("\\", "/"));

            return NSFileHandle.OpenRead(filePath.Replace("\\", "/")).ReadDataToEndOfFile().AsStream();
        }
    }
}
