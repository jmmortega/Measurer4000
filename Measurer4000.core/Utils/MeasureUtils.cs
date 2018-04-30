using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Measurer4000.Core.Models;
using Measurer4000.Core.Services.Interfaces;

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
            StreamReader reader = null;
            int count = 0;
            int inComment = 0;

            try
            {
                reader = new StreamReader(File.OpenRead(programmingFile.Path));
                string line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    if(IsRealCodeCSharp(line.Trim(), ref inComment))
                    {
                        count++;
                    }
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {e.Message}");
            }
            finally
            {
                if(reader != null)
                {
                    reader.Dispose();
                }
            }

            return count;
        }

        private static long CalculateLOCXaml(ProgrammingFile programmingFile)
        {
            StreamReader reader = null;
            int count = 0;
            int inComment = 0;

            try
            {
                reader = new StreamReader(File.OpenRead(programmingFile.Path));
                string line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    if (IsRealCodeXaml(line.Trim(), ref inComment))
                    {
                        count++;
                    }
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {e.Message}");
            }
            finally
            {
                if(reader != null)
                {
                    reader.Dispose();
                }
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

        private static bool IsRealCodeAxml(string line, ref int inComment)
        {
            return IsRealCodeXaml(line, ref inComment);
        }

        private static bool IsRealCodeXaml(string line, ref int inComment)
        {
            if(line.StartsWith("<!--") && line.EndsWith("-->"))
            {
                return false;
            }
            else if(line.StartsWith("<!--"))
            {
                inComment++;
                return false;
            }
            else if(line.EndsWith("-->"))
            {
                inComment--;
                return false;
            }

            return
                inComment == 0
                || line.Contains("/>")                
                || line.Contains(">");
        }

        private static bool IsRealCodeXib(string line, ref int inComment)
        {
            return IsRealCodeXaml(line, ref inComment);
        }

        private static bool IsRealCodeCSharp(string line, ref int inComment)
        {
            if(line.StartsWith("/*") && line.EndsWith("*/"))
            {
                return false;
            }
            else if (line.StartsWith("/*"))
            {
                inComment++;
                return false;
            }
            else if(line.EndsWith("*/"))
            {
                inComment--;
                return false;
            }

            return
                inComment == 0
            && !line.StartsWith("//")
            && (line.StartsWith("if")
            || line.StartsWith("else if")
            || line.StartsWith("using")
            || line.Contains(";")
            || line.StartsWith("public")
            || line.StartsWith("private")
            || line.StartsWith("protected"));
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
                TotalLinesCore = _currentSolution.Projects.Where(x => x.Platform == EnumPlatform.PCL).SelectMany(x => x.Files).Sum(x => x.LOC),            
            };

            stats.iOSSpecificCode = Math.Round(100 - stats.ShareCodeIniOS, 2);
            stats.AndroidSpecificCode = Math.Round(100 - stats.ShareCodeInAndroid, 2);
			stats.UWPSpecificCode = Math.Round(100 - stats.ShareCodeInUWP, 2);

            return stats;
        }

		private static double CalculateShareCodePerPlaform(Solution _currentSolution, EnumPlatform _selectedPlatform)
		{
			var totalCoreLOC = _currentSolution.Projects.Where(x => x.Platform == EnumPlatform.PCL || x.Platform == EnumPlatform.NetStandard).SelectMany(x => x.Files).Where(x => x.IsUserInterface == false).Sum(x => x.LOC);
			var platformSpecificLOC = _currentSolution.Projects.Where(x => x.Platform == _selectedPlatform).SelectMany(x => x.Files).Where(x => x.IsUserInterface == false).Sum(x => x.LOC);

			return Math.Round(((double) totalCoreLOC / (platformSpecificLOC + totalCoreLOC)) * 100, 2);
		}
    }
}
