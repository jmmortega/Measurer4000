using System.Collections.Generic;

namespace Measurer4000.Core.Models
{
    public class Solution
    {
        public Solution()
        {
            Projects = new List<Project>();
            Stats = new CodeStats();
        }

        public List<Project> Projects { get; set; }

        public CodeStats Stats { get; set; }

        public override string ToString()
        {
            var str = string.Empty;

            foreach(var project in Projects)
            {
                str += project.ToString();
            }

            str += Stats.ToString();

            return str;
        }
    }
}
