using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Measurer4000.Core.Utils.Interfaces
{
    interface IFileManager
    {
        Stream OpenRead(string filePath);
    }
}
