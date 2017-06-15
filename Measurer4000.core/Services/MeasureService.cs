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
        public IFileManagerService FileManagerService
        {
            get { return _fileManagerService; }
            set { _fileManagerService = value; }
        }

        public Solution GetProjects(string filePathToSolution)
        {
            ProjectIdentificatorUtils.File = FileManagerService;
            List<string> ProjectLines = ProjectIdentificatorUtils.ReadProjectsLines(filePathToSolution);
            List<Project> SolutionProjects = ProjectIdentificatorUtils.TranslateProjectsLinesToProjects(ProjectLines);
            Parallel.ForEach(SolutionProjects, (hit) => {
                ProjectIdentificatorUtils.CompleteInfoForProject(hit, filePathToSolution);
            });
            return new Solution() { Projects = SolutionProjects };
        }

        public Solution Measure(Solution solution)
        {
            MeasureUtils.File = FileManagerService;
            Parallel.ForEach(solution.Projects, (project) =>
            {
                Parallel.ForEach(project.Files, (programmingFile) =>
                {
                    programmingFile.LOC = MeasureUtils.CalculateLOC(programmingFile);
                });
            });
            return solution;
        }
    }
}
