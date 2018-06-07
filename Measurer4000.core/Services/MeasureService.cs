using Measurer4000.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using Measurer4000.Core.Models;
using Measurer4000.Core.Utils;
using System.Threading.Tasks;

namespace Measurer4000.Core.Services
{
    public class MeasureService : IMeasurerService
    {
        private IFileManagerService _fileManagerService;

        public MeasureService(IFileManagerService file) { _fileManagerService = file;}

        public Solution GetProjects(string filePathToSolution)
        {
            ProjectIdentificatorUtils.File = _fileManagerService;
            List<string> ProjectLines = ProjectIdentificatorUtils.ReadProjectsLines(filePathToSolution);
            List<Project> SolutionProjects = ProjectIdentificatorUtils.TranslateProjectsLinesToProjects(ProjectLines);
            Parallel.ForEach(SolutionProjects, (hit) => {
                ProjectIdentificatorUtils.CompleteInfoForProject(hit, filePathToSolution);
            });
            return new Solution() { Projects = SolutionProjects };
        }

        public Solution Measure(Solution solution)
        {
            MeasureUtils.File = _fileManagerService;
            Parallel.ForEach(solution.Projects, (project) =>
            {
                Parallel.ForEach(project.Files, (programmingFile) =>
                {
                    programmingFile.TypeFile = DetectFileType(programmingFile.Name);
                    programmingFile.LOC = MeasureUtils.CalculateLOC(programmingFile);
                });
            });

            solution.Stats = MeasureUtils.CalculateStats(solution);
            return solution;
        }

        public EnumTypeFile DetectFileType(string filePath)
        {
            if (filePath.EndsWith(".axml"))
                return EnumTypeFile.AXML;
            else if (filePath.EndsWith(".cs"))
                return EnumTypeFile.CSharp;
            else if (filePath.EndsWith(".xaml"))
                return EnumTypeFile.XAML;
            else if (filePath.EndsWith(".xib"))
                return EnumTypeFile.Xib;

            return EnumTypeFile.Indeterminate;
        }


    }
}
