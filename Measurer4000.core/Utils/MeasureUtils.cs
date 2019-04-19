using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Measurer4000.Core.Models;
using Measurer4000.Core.Services.Interfaces;
using System.Text.RegularExpressions;

namespace Measurer4000.Core.Utils
{
	public static class MeasureUtils
    {
        public static IFileManagerService File;
        public static long CalculateLOC(ProgrammingFile programmingFile)
        {
            if(programmingFile.TypeFile == EnumTypeFile.AXML)
            {
                return CalculateLOCAxml(programmingFile);
            }
            else if(programmingFile.TypeFile == EnumTypeFile.CSharp)
            {
                return CalculateLOCSharp(programmingFile);
            }
            else if(programmingFile.TypeFile == EnumTypeFile.XAML)
            {
                return CalculateLOCXaml(programmingFile);
            }
            else if (programmingFile.TypeFile == EnumTypeFile.Xib)
            {
                return CalculateLOCXib(programmingFile);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"File not recognized {programmingFile.Path}");
                return 0;
            }
        }

        private static long CalculateLOCSharp(ProgrammingFile programmingFile)
        {
            int count = 0;
            try
            {
                using (StreamReader reader = new StreamReader(File.OpenRead(programmingFile.Path)))
                {
                    count = IsRealCodeCSharp(reader.ReadToEnd());
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {e.Message}");
            }
            return count;
        }

        private static long CalculateLOCXaml(ProgrammingFile programmingFile)
        {
            int count = 0;
            try
            {
                using (StreamReader reader = new StreamReader(File.OpenRead(programmingFile.Path)))
                {
                    count = IsRealCodeXaml(reader.ReadToEnd());
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {e.Message}");
            }
            return count;
        }

        private static long CalculateLOCAxml(ProgrammingFile programmingFile)
        {
            return CalculateLOCXaml(programmingFile);
        }

        private static long CalculateLOCXib(ProgrammingFile programmingFile)
        {
            return CalculateLOCXaml(programmingFile);
        }

        private static int IsRealCodeAxml(string fileContent)
        {
            return IsRealCodeXaml(fileContent);
        }

        private static int IsRealCodeXaml(string fileContent)
        {
            var pattern = "(<\\w+)";
            return Regex.Matches(fileContent, pattern).Count;
        }

        private static int IsRealCodeXib(string fileContent)
        {
            return IsRealCodeXaml(fileContent);
        }

        private static int IsRealCodeCSharp(string fileContent)
        {
            var comments = "(\\s*//.+)|/\\*(?s:.*?)\\*/|(\\s*(?:\\{|\\}))";
            var blank = "(^\\s*$\\n|\\r)";
            var codePattern = @"(.+;(\n|\r|\r\n)|public|protected|internal|private|if([\(|\s\(])|else if([\(|\s\(])|while([\(|\s\(])|for([\(|\s\(])|foreach([\(|\s\(])|(\[(.+)\]$))";
            
            var code = Regex.Replace(fileContent, comments, "");
            code = Regex.Replace(code, blank, "", RegexOptions.Multiline);

            return Regex.Matches(code, codePattern).Count;
        }

        public static CodeStats CalculateStats(Solution _currentSolution)
        {
			var files = _currentSolution.Projects.Where(x => x.Platform == EnumPlatform.Android).SelectMany(x => x.Files);

			foreach (var item in files)
			{
				Debug.WriteLine($"file: {item.Name}");
			}

			var stats = new CodeStats()
            {
				ShareCodeInAndroid = CalculateShareCodePerPlaform(_currentSolution, EnumPlatform.Android),
                
				AndroidFiles = files.Count(),
				TotalLinesInAndroid = _currentSolution.Projects.Where(x => x.Platform == EnumPlatform.Android).SelectMany(x => x.Files).Sum(x => x.LOC),

				ShareCodeIniOS = CalculateShareCodePerPlaform(_currentSolution, EnumPlatform.iOS),
				iOSFiles = _currentSolution.Projects.Where(x => x.Platform == EnumPlatform.iOS).SelectMany(x => x.Files).Count(),
				TotalLinesIniOS = _currentSolution.Projects.Where(x => x.Platform == EnumPlatform.iOS).SelectMany(x => x.Files).Sum(x => x.LOC),

				ShareCodeInUWP = CalculateShareCodePerPlaform(_currentSolution, EnumPlatform.UWP),
				UWPFiles = _currentSolution.Projects.Where(x => x.Platform == EnumPlatform.UWP).SelectMany(x => x.Files).Count(),
				TotalLinesInUWP = _currentSolution.Projects.Where(x => x.Platform == EnumPlatform.UWP).SelectMany(x => x.Files).Sum(x => x.LOC),

                AmountOfFiles = _currentSolution.Projects.SelectMany(p => p.Files).Count(),
                CodeFiles = _currentSolution.Projects.SelectMany(x => x.Files).Count(x => x.IsUserInterface == false),
                UIFiles = _currentSolution.Projects.SelectMany(x => x.Files).Count(x => x.IsUserInterface == true),
                TotalLinesOfCode = _currentSolution.Projects.SelectMany(x => x.Files).Where(x => x.IsUserInterface == false).Sum(x => x.LOC),
                TotalLinesOfUI = _currentSolution.Projects.SelectMany(x => x.Files).Where(x => x.IsUserInterface == true).Sum(x => x.LOC),
                TotalLinesCore = _currentSolution.Projects.Where(x => x.Platform == EnumPlatform.Core).SelectMany(x => x.Files).Sum(x => x.LOC),            
            };

            stats.iOSSpecificCode = Math.Round(100 - stats.ShareCodeIniOS, 2);
            stats.AndroidSpecificCode = Math.Round(100 - stats.ShareCodeInAndroid, 2);
			stats.UWPSpecificCode = Math.Round(100 - stats.ShareCodeInUWP, 2);

            return stats;
        }

		private static double CalculateShareCodePerPlaform(Solution _currentSolution, EnumPlatform _selectedPlatform)
		{
			var totalCoreLOC = _currentSolution.Projects.Where(x => x.Platform == EnumPlatform.Core).SelectMany(x => x.Files).Where(x => x.IsUserInterface == false).Sum(x => x.LOC);
			var platformSpecificLOC = _currentSolution.Projects.Where(x => x.Platform == _selectedPlatform).SelectMany(x => x.Files).Where(x => x.IsUserInterface == false).Sum(x => x.LOC);

			return Math.Round(((double) totalCoreLOC / (platformSpecificLOC + totalCoreLOC)) * 100, 2);
		}
    }
}
