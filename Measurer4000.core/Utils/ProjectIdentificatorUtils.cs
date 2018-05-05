﻿using Measurer4000.Core.Models;
using Measurer4000.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace Measurer4000.Core.Utils
{
    public static class ProjectIdentificatorUtils
    {
        public static IFileManagerService File;
        public static List<string> ReadProjectsLines(string filePathToSolution)
        {
            var projects = new List<string>();
            StreamReader solutionReader = null;

            try
            {                
                solutionReader = new StreamReader(File.OpenRead(filePathToSolution)); 
                while(!solutionReader.EndOfStream)
                {
                    string line = solutionReader.ReadLine();
					if (line.StartsWith("Project") && line.Contains(".csproj")) projects.Add(line);
                    System.Diagnostics.Debug.WriteLine(line+(line.StartsWith("Project")?" is project":" isnt project"));
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
                    Name = thatINeedSplitted[0].Trim().Trim('"'),
                    Path = thatINeedSplitted[1].Trim().Trim('"').Replace("\\", "/")
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
                    Path.GetDirectoryName(pathToSolution), project.Path)));

                while(!projectReader.EndOfStream)
                {
                    string line = projectReader.ReadLine();
                    if (project.Platform == EnumPlatform.None)
                    {
                        project.Platform = ThisLineDeterminePlatform(line);
                    }

                    if (line.Contains("Compile") || line.Contains("InterfaceDefinition") || line.Contains("AndroidResource"))
                    {
                        if (ItsAValidFile(line))
                        {
                            project.Files.Add(WellSmellsLikeFile(line, Path.GetDirectoryName(
                                (Path.Combine(Path.GetDirectoryName(pathToSolution), project.Path)))));
                            System.Diagnostics.Debug.WriteLine(line + (project.Files[project.Files.Count-1].IsUserInterface ? " is" : " isnt")+" UI");
                        }                     
                    }
                }
                System.Diagnostics.Debug.WriteLine(project.Name + " is " + project.Platform);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            finally
            {
                if(projectReader != null)
                {
                    projectReader.Dispose();
                }
            }

            return project;
        }

        private static EnumPlatform ThisLineDeterminePlatform(string line)
        {
			if(line.ToLower().Contains("androidmanifest.xml"))
            {
                return EnumPlatform.Android;
            }
            else if(line.ToLower().Contains("mtouch"))
            {
                return EnumPlatform.iOS;
            }
			else if (line.ToLower().Contains("<targetplatformidentifier>uap</targetplatformidentifier>"))
            {
				return EnumPlatform.UWP;
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

        private static bool ItsAValidFile(string fileLine)
        {
            return (fileLine.Contains(".cs") || fileLine.Contains(".xaml") || fileLine.Contains(".axml") || fileLine.Contains(".xib") || fileLine.Contains(".xml")) && !fileLine.Contains("Resource.Designer.cs");
        }

        private static ProgrammingFile WellSmellsLikeFile(string includeFileLine, string projectsPath)
        {                        
            string pathFile = includeFileLine.Split('=')[1].Trim('>').Trim('/').Trim('"').Trim().Trim('"');

            return new ProgrammingFile()
            {
                Name = pathFile,
                Path = Path.Combine(projectsPath, pathFile),
                IsUserInterface = pathFile.Contains(".axml") || pathFile.Contains(".designer") || pathFile.Contains(".xaml") || pathFile.Contains(".xib")
            };
        }

    }

}
