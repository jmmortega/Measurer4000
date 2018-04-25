using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Measurer4000.Core.Models
{
    public class CodeStats
    {
        public double ShareCodeInAndroid { get; set; }

        public double ShareCodeIniOS { get; set; }

		public double ShareCodeInUWP { get; set; }

        public double AndroidSpecificCode { get; set; }

        public double iOSSpecificCode { get; set; }

		public double UWPSpecificCode { get; set; }

        public long AmountOfFiles { get; set; }

        public long CodeFiles { get; set; }

        public long UIFiles { get; set; }

        public long TotalLinesOfCode { get; set; }

        public long TotalLinesOfUI { get; set; }

        public long AndroidFiles { get; set; }

        public long iOSFiles { get; set; }

		public long UWPFiles { get; set; }

        public long TotalLinesCore { get; set; }

        public long TotalLinesInAndroid { get; set; }

        public long TotalLinesIniOS { get; set; }

		public long TotalLinesInUWP { get; set; }
    }
}
