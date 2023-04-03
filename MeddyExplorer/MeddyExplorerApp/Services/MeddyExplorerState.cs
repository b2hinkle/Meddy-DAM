using MeddyExplorerLibrary;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeddyExplorerApp.Services
{
    internal class MeddyExplorerState
    {
        public List<FileSystemInfo> Files { get; set; } = new();

        public bool DrawerOpen = false;

        public delegate void DirectoryParametersDelegate(DirectoryInfo in1, DirectoryInfo in2);
        public event DirectoryParametersDelegate OnRootDirChangedDelegate;
        public event DirectoryParametersDelegate OnCurrentDirChangedDelegate;
        private DirectoryInfo _rootDir { get; set; }
        public DirectoryInfo RootDir
        {
            get { return _rootDir; }
            set
            {
                DirectoryInfo oldValue = _rootDir;
                _rootDir = value;

                if (OnRootDirChangedDelegate is not null)
                {
                    OnRootDirChangedDelegate.Invoke(oldValue, _rootDir);
                }
            }
        }
        private DirectoryInfo _currentDir;
        public DirectoryInfo CurrentDir
        {
            get { return _currentDir; }
            set
            {
                DirectoryInfo oldValue = _currentDir;
                _currentDir = value;

                if (OnCurrentDirChangedDelegate is not null)
                {
                    OnCurrentDirChangedDelegate.Invoke(oldValue, _currentDir);
                }
            }
        }

        public MeddyExplorerState()
        {
            OnRootDirChangedDelegate += OnRootDirInitialized;
            OnCurrentDirChangedDelegate += OnCurrentDirChanged;
        }

        private void OnRootDirInitialized(DirectoryInfo inOldRootDir, DirectoryInfo inNewRootDir)
        {
            OnRootDirChangedDelegate -= OnRootDirInitialized; // we're only meant to run to initialize things

            if (inOldRootDir is null)
            {
                App.persistentData.AddNewRecentMeddyProject(inNewRootDir);
                CurrentDir = inNewRootDir;
            }
        }

        protected void OnCurrentDirChanged(DirectoryInfo inOldRootDir, DirectoryInfo inNewRootDir)
        {
            MELFileSystemFunctionLibrary.PopulateFileSystemInfoList(Files, CurrentDir.FullName);
        }
    }
}
