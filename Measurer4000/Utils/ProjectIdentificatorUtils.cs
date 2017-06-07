using Measurer4000.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Measurer4000.Utils
{
    public static class ProjectIdentificatorUtils
    {
        public static List<string> ReadProjectsLines(string filePathToSolution)
        {
            var projects = new List<string>();
            StreamReader solutionReader = null;

            try
            {                
                solutionReader = new StreamReader(File.OpenRead(filePathToSolution));
                
                while(solutionReader.EndOfStream)
                {
                    string line = solutionReader.ReadLine();

                    if(line.Contains("Project") && !line.Contains("EndProject"))
                    {
                        projects.Add(line);           
                    }
                }                
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            finally
            {
                if(solutionReader != null)
                {
                    solutionReader.Close();
                    solutionReader.Dispose();
                }
            }
                        
            return projects;
        }

        public static List<Project> TranslateProjectsLinesToProjects(List<string> projectLines)
        {
            var projects = new List<Project>();
                        
            foreach(var projectLine in projectLines)
            {
                string thatINeed = projectLine.Split('=')[1].Trim();                
                string[] thatINeedSplitted = thatINeed.Split(',');

                projects.Add(new Project()
                {
                    Name = thatINeedSplitted[0],
                    Path = thatINeedSplitted[1]
                });                
            }

            return projects;
        }

        public static Project CompleteInfoForProject(Project project, string pathToSolution)
        {
            StreamReader projectReader = null;
                        
            try
            {
                projectReader = new StreamReader(File.OpenRead(Path.Combine(
                    new FileInfo(pathToSolution).Directory.FullName,
                    project.Path)));

                while(projectReader.EndOfStream)
                {
                    string line = projectReader.ReadLine();
                    if (project.Platform == EnumPlatform.None)
                    {
                        project.Platform = ThisLineDeterminePlatform(line);
                    }

                    if(line.Contains("Compile") || line.Contains("InterfaceDefinition") || line.Contains("AndroidResource"))
                    {
                        if(ItsAValidFile(line))
                        {
                            project.Files.Add(WellSmellsLikeFile(line));
                        }                        
                    }
                }                
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            finally
            {
                if(projectReader != null)
                {
                    projectReader.Close();
                    projectReader.Dispose();
                }
            }

            return project;
        }

        public static EnumPlatform ThisLineDeterminePlatform(string line)
        {
            if(line.ToLower().Contains("android"))
            {
                return EnumPlatform.Android;
            }
            else if(line.ToLower().Contains("mtouch"))
            {
                return EnumPlatform.iOS;
            }
            //I suppose only have PCL libraries in core project. (Obviously?)
            else if(line.ToLower().Contains("targetframeworkprofile"))
            {
                return EnumPlatform.Core;
            }
            else
            {
                return EnumPlatform.None;
            }
        }

        public static bool ItsAValidFile(string fileLine)
        {
            return fileLine.Contains(".cs") || fileLine.Contains(".xaml") || fileLine.Contains(".axml") || fileLine.Contains(".xib") || fileLine.Contains(".xml");
        }

        public static ProgrammingFile WellSmellsLikeFile(string includeFileLine)
        {                        
            string pathFile = includeFileLine.Split('=')[1];

            return new ProgrammingFile()
            {
                Name = pathFile,
                IsUserInterface = pathFile.Contains(".axml") || pathFile.Contains(".designer") || pathFile.Contains(".xaml") || pathFile.Contains(".xib")
            };
        }

        
    }

}
