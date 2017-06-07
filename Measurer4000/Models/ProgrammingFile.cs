namespace Measurer4000.Models
{
    public class ProgrammingFile
    {
        public string Name { get; set; }

        public long LOC { get; set; }

        public bool IsUserInterface { get; set; }

        public EnumTypeFile TypeFile { get; set; }

        public string Path { get; set; }
    }
}
