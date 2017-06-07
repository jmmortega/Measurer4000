using Measurer4000.Models;
using System.Collections.Generic;

namespace Measurer4000.Services.Interfaces
{
    public interface IMeasurerService
    {
        Solution GetProjects(string filePathToSolution);

        Solution Measure(Solution SolutionProjects);

    }
}
