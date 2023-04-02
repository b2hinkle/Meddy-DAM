namespace MeddyExplorerApp;

public partial class App : Application
{
    public static MEAPersistentData persistentData { get; set; } = new MEAPersistentData();

    public App()
	{
		InitializeComponent();

		MainPage = new MainPage();
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        Window window = base.CreateWindow(activationState);

        window.Created += persistentData.LoadFromDisk;
        window.Stopped += persistentData.SaveToDisk;

        return window;
    }
}

public class MEAPersistentData
{
    private List<string> _recentMeddyProjects = new List<string>(); // maybe use a set or something that inherently doesn't contain duplicates?
    public List<string> RecentMeddyProjects
    {
        get { return _recentMeddyProjects; }
        private set { _recentMeddyProjects = value; }
    }

    public void AddNewRecentMeddyProject(DirectoryInfo inNewRootDir)
    {
        if (RecentMeddyProjects.Count > 0)
        {
            if (RecentMeddyProjects.First() == inNewRootDir.FullName) // no need to add if it's already on the top of the list
            {
                return;
            }
            if (RecentMeddyProjects.Contains(inNewRootDir.FullName)) // remove it from the list before we add it to the top to ensure we don't have duplicates
            {
                RecentMeddyProjects.Remove(inNewRootDir.FullName);
            }
        }

        RecentMeddyProjects.Insert(0, inNewRootDir.FullName);
    }

    public void LoadFromDisk(object sender, EventArgs e)
    {
        string fileToLoad = Path.Combine(FileSystem.AppDataDirectory, "PersistentData.xml");
        MEAPersistentData loadedPersistentDataObject = MeddyExplorerLibrary.MELFileSystemFunctionLibrary.Load<MEAPersistentData>(fileToLoad);
        if (loadedPersistentDataObject is not null)
        {
            RecentMeddyProjects = loadedPersistentDataObject.RecentMeddyProjects;
        }
        
    }
    public void SaveToDisk(object sender, EventArgs e)
    {
        string newFileToCreate = Path.Combine(FileSystem.AppDataDirectory, "PersistentData.xml");
        MeddyExplorerLibrary.MELFileSystemFunctionLibrary.Save<MEAPersistentData>(newFileToCreate, this);
    }
}
