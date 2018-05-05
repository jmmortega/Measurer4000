using System.Linq;
using Measurer4000.Addin.Windows;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;

namespace Measurer4000.Addin.Commands
{
    public class MeasurerCommand : CommandHandler
    {
        protected override void Update(CommandInfo info)
        {
            info.Visible = true;

            var projects = IdeApp.Workspace.GetAllProjects();

            if (projects.Any())
                info.Enabled = true;
            else
                info.Enabled = false;
        }

        protected override void Run()
        {
            new MeasurerWindow().Show();
        }
    }
}