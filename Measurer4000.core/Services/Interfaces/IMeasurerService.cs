using Measurer4000.Core.Models;
using System.Collections.Generic;

namespace Measurer4000.Core.Services.Interfaces
{
    public interface IMeasurerService
    {
        Solution GetProjects(string filePathToSolution);

        Solution Measure(Solution SolutionProjects);
    }
}
