using Measurer4000.Core.Models;

namespace Measurer4000.Core.Utils
{
    public static class ShareCodeReportUtils
    {
        private const string BASEURL = @"https://docs.google.com/forms/d/e/1FAIpQLSe1CMNFNnAh_GoZ3z9PD7d5a07CUd9zOVk3sywURY__zHMytA/viewform?entry.187884389={0}&entry.1649909781={1}&entry.1981106453={2}&entry.827223542={3}&entry.1102590771={4}&entry.2068980640&entry.767914800&entry.1673597349&entry.1583496322";


        public static string CreateShareUrl(CodeStats stats)
        {
            return string.Format(BASEURL, stats.ShareCodeIniOS, stats.ShareCodeInAndroid, stats.iOSSpecificCode, stats.AndroidSpecificCode, stats.TotalLinesOfCode);            
        }
    }
}
