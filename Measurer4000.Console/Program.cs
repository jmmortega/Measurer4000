using Measurer4000.Command.Services;
using Measurer4000.Core.Models;
using Measurer4000.Core.Services;
using Measurer4000.Core.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;


namespace Measurer4000.Command
{
    class Program
    {
        static void Main(string[] args)
        {
            InitServices();
            if(args.Count() == 0)
            {
                PrintHelp();
            }
            else
            {
                var arguments = new MeasurerParameters(args);

                if(string.IsNullOrEmpty(arguments.SolutionFileName))
                {
                    Console.WriteLine("Solution filename not specified");
                }
                else
                {
                    Measure(arguments);
                }                
            }

#if DEBUG
            Console.WriteLine("Finito");
            Console.ReadKey();
#endif
        }

        private static void Measure(MeasurerParameters arguments)
        {
            var measurerService = ServiceLocator.Get<IMeasurerService>();

            Console.WriteLine("Getting projects");

            var projects = measurerService.GetProjects(arguments.SolutionFileName);

            Console.WriteLine("Measuring...");

            var stats = measurerService.Measure(projects);

            if (arguments.IsJson)
            {
                JsonStats(stats, arguments.Complete);
            }
            else
            {
                PrintStats(stats, arguments.Complete);
            }
        }

        private static void PrintHelp()
        {
            Console.WriteLine(@"Measurer 4000 https://github.com/jmmortega/Measurer4000 
                              Sample: Measurer4000 -json -complete SolutionPath.sln 
                              -json: To receive json file with measurer data 
                              -complete: To receive all data about measuring 
                              SolutionPath don't forget .sln extension!");
        }

        private static void JsonStats(Solution stats, bool complete)
        {
            string json = string.Empty;

            if(complete)
            {
                json = JsonConvert.SerializeObject(stats);
            }
            else
            {
                json = JsonConvert.SerializeObject(stats.Stats);
            }

            StreamWriter writer = new StreamWriter("stats.json");
            writer.WriteLine(json);
            writer.Close();
        }

        private static void PrintStats(Solution stats, bool complete)
        {
            if(complete)
            {
                Console.WriteLine(stats.ToString());
            }
            else
            {
                Console.WriteLine(stats.Stats.ToString());
            }
        }

        private static void InitServices()
        {
            var fileManagerService = new FileManagerService();
            ServiceLocator.Register<IFileManagerService>(fileManagerService);
            ServiceLocator.Register<IMeasurerService>(new MeasureService(fileManagerService));
        }
    }
}
