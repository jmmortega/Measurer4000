using Measurer4000.Services.Interfaces;
using System;
using System.Collections.Generic;
using Measurer4000.Models;
using Measurer4000.Utils;

namespace Measurer4000.Services
{
    public class MeasureService : IMeasurerService
    {
        public Solution GetProjects(string filePathToSolution)
        {
            List<string> ProjectLines = ProjectIdentificatorUtils.ReadProjectsLines(filePathToSolution);
            List<Project> SolutionProjects = ProjectIdentificatorUtils.TranslateProjectsLinesToProjects(ProjectLines);
            foreach (Project hit in SolutionProjects){
                ProjectIdentificatorUtils.CompleteInfoForProject(hit, filePathToSolution);
            }
            return new Solution() { Projects = SolutionProjects };
        }

        public Solution Measure(Solution solution)
        {
            foreach (Project project in solution.Projects) {
                foreach (ProgrammingFile programmingFile in project.Files) {
                    programmingFile.LOC = MeasureUtils.CalculateLOC(programmingFile);
                }
            }
            return solution;
        }
    }
}
