namespace Measurer4000.Core.Models
{
    public class CodeStats
    {
        public double ShareCodeInAndroid { get; set; }

        public double ShareCodeIniOS { get; set; }

        private double shareCodeInUWP;

		public double ShareCodeInUWP 
        { 
            get
            {
                if(double.IsNaN(shareCodeInUWP))
                {
                    return 0;
                }
                return shareCodeInUWP;
            }
            set => shareCodeInUWP = value;
        }

        public double AndroidSpecificCode { get; set; }

        public double iOSSpecificCode { get; set; }

        private double uwpSpecificCode;


		public double UWPSpecificCode 
        { 
            get
            {
                if(double.IsNaN(uwpSpecificCode))
                {
                    return 0;
                }
                return uwpSpecificCode;
            }
            set
            {
                uwpSpecificCode = value;
            }
        }

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

        public override string ToString()
        {
            return $@"Share code Android: {ShareCodeInAndroid} 
                        Share code iOS: {ShareCodeIniOS} 
                        Share code UWP: {ShareCodeInUWP} 
                        Android specific: {AndroidSpecificCode} 
                        iOS specific: {iOSSpecificCode} 
                        UWP specific: {UWPSpecificCode} 
                        Files: {AmountOfFiles} 
                        Code files: {CodeFiles} 
                        UI files: {UIFiles} 
                        Total lines of code {TotalLinesOfCode} 
                        Total lines of UI {TotalLinesOfUI} 
                        Android files {AndroidFiles} 
                        iOS files {iOSFiles} 
                        UWP files {UWPFiles} 
                        Total lines core {TotalLinesCore} 
                        Total lines Android {TotalLinesInAndroid} 
                        Total lines iOS {TotalLinesIniOS} 
                        Total lines in UWP {TotalLinesInUWP} ";                        
        }
    }
}
