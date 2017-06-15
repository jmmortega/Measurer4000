using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Measurer4000.Core.Services.Interfaces
{
    public interface IFileManagerService
    {
        Stream OpenRead(string filePath);
    }
}
