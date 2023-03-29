namespace MeddyExplorerApp.Models
{
    internal class FileModelFrontEnd
    {
        public FileModelFrontEnd(string inName, string inDateModified, string inType, string inSize) 
        { 
            Name = inName;
            DateModified = inDateModified;
            Type = inType;
            Size = inSize;
        }
        public string Name { get; set; }
        public string DateModified { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
    }
}
