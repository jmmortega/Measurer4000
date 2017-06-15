using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Measurer4000.Core.Services.Interfaces;
namespace Measurer4000.Services
{
    class FileManagerService : IFileManagerService
    {
        public Stream OpenRead(string filePath)
        {
            return File.OpenRead(filePath);
        }
    }
}
