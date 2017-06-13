using Measurer4000.core.Models;
using System.Collections.Generic;

namespace Measurer4000.core.Services.Interfaces
{
    public interface IMeasurerService
    {
        Solution GetProjects(string filePathToSolution);

        Solution Measure(Solution SolutionProjects);
    }
}
