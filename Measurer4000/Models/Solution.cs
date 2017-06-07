using System.Collections.Generic;

namespace Measurer4000.Models
{
    public class Solution
    {
        public Solution()
        {
            Projects = new List<Project>();
        }

        public List<Project> Projects { get; set; }
    }
}
