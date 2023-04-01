namespace MeddyExplorerApp.Objects
{
    public class MeddyExplorerSessionInfo
    {
        public delegate void DirectoryParametersDelegate(DirectoryInfo in1, DirectoryInfo in2);
        public event DirectoryParametersDelegate OnRootDirChanged;
        public event DirectoryParametersDelegate OnCurrentDirChanged;

        private DirectoryInfo rootDir;
        public DirectoryInfo RootDir
        {
            get { return rootDir; }
            set
            {
                DirectoryInfo oldRootDir = rootDir;
                rootDir = value;

                if (OnRootDirChanged is not null)
                {
                    OnRootDirChanged.Invoke(oldRootDir, rootDir);
                }
            }
        }
        private DirectoryInfo currentDir;
        public DirectoryInfo CurrentDir
        {
            get { return currentDir; }
            set
            {
                DirectoryInfo oldCurrentDir = currentDir;

                currentDir = value;

                if (OnCurrentDirChanged is not null)
                {
                    OnCurrentDirChanged.Invoke(oldCurrentDir, currentDir);
                }
            }
        }

        public MeddyExplorerSessionInfo()
        {
            OnRootDirChanged += (inOldRootDir, inNewRootDir) => App.persistentData.AddToRecentMeddyProjects(inNewRootDir);
        }
    }
}