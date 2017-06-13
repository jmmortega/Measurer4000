using Measurer4000.core.Services.Interfaces;
using System;
using System.Collections.Generic;
using Measurer4000.core.Models;
using Measurer4000.core.Utils;
using System.Threading.Tasks;

namespace Measurer4000.core.Services
{
    public class MeasureService : IMeasurerService
    {
        public Solution GetProjects(string filePathToSolution)
        {
            List<string> ProjectLines = ProjectIdentificatorUtils.ReadProjectsLines(filePathToSolution);
            List<Project> SolutionProjects = ProjectIdentificatorUtils.TranslateProjectsLinesToProjects(ProjectLines);
            Parallel.ForEach(SolutionProjects, (hit) => {
                ProjectIdentificatorUtils.CompleteInfoForProject(hit, filePathToSolution);
            });
            return new Solution() { Projects = SolutionProjects };
        }

        public Solution Measure(Solution solution)
        {
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
