using System.Linq;

namespace Measurer4000.Command
{
    public class MeasurerParameters
    {
        public MeasurerParameters(string[] args)
        {
            SolutionFileName = args.FirstOrDefault(x => x.Contains(".sln"));            
            IsJson = args.Any(x => x.Contains("-json"));
            Complete = args.Any(x => x.Contains("-complete"));
        }

        public string SolutionFileName { get; set; }

        public bool IsJson { get; set; }

        public bool Complete { get; set; }


    }
}
