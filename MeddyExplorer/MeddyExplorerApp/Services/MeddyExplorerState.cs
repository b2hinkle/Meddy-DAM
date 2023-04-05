using MeddyExplorerLibrary;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeddyExplorerApp.Services
{
    internal class MeddyExplorerState : IDisposable
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

        public event EventHandler<FileSystemInfo> OnFileTargeted;
        public event EventHandler<FileSystemInfo> OnFileUntargeted;

        private FileSystemInfo _targetFile;
        /// <summary>
        /// The file system item that the user has targeted.
        /// Not necessarily selected.
        /// </summary>
        public FileSystemInfo TargetFile
        {
            get
            {
                return _targetFile;
            }
            set
            {
                FileSystemInfo oldValue = _targetFile;
                _targetFile = value;

                if (OnFileUntargeted is not null)
                {
                    if (oldValue is not null)
                    {
                        OnFileUntargeted.Invoke(this, oldValue);
                    }
                }
                if (OnFileTargeted is not null)
                {
                    if (_targetFile is not null)
                    {
                        OnFileTargeted.Invoke(this, _targetFile);
                    }
                }
            }
        }

        public event EventHandler OnSelectedFilesChanged;
        public event EventHandler<FileSystemInfo> OnFileSelected;
        public event EventHandler<FileSystemInfo> OnFileDeselected;

        /// <summary>
        /// List of file system items that the user has selected.
        /// </summary>
        protected List<FileSystemInfo> SelectedFiles { get; set; }

        public ReadOnlyCollection<FileSystemInfo> GetSelectedFiles()
        {
            return SelectedFiles.AsReadOnly();
        }

        public void AddToSelectedFiles(FileSystemInfo inFileSystemInfo)
        {
            SelectedFiles.Add(inFileSystemInfo);

            if (OnFileSelected is not null)
            {
                OnFileSelected.Invoke(this, inFileSystemInfo);
            }
            if (OnSelectedFilesChanged is not null)
            {
                OnSelectedFilesChanged.Invoke(this, EventArgs.Empty);
            }
        }
        public void RemoveFromSelectedFiles(FileSystemInfo inFileSystemInfo)
        {
            SelectedFiles.Remove(inFileSystemInfo);

            if (OnFileDeselected is not null)
            {
                OnFileDeselected.Invoke(this, inFileSystemInfo);
            }
            if (OnSelectedFilesChanged is not null)
            {
                OnSelectedFilesChanged.Invoke(this, EventArgs.Empty);
            }
        }
        public void ClearSelectedFiles()
        {
            foreach (FileSystemInfo fileSystemInfo in new List<FileSystemInfo>(SelectedFiles))
            {
                RemoveFromSelectedFiles(fileSystemInfo);
            }
        }

        // To be called from the component
        public void Initialize(string inRootDir)
        {
            OnCurrentDirChangedDelegate += OnCurrentDirChanged;
            RootDir = new DirectoryInfo(inRootDir);
            SelectedFiles = new List<FileSystemInfo>();
            App.persistentData.AddNewRecentMeddyProject(RootDir);
            CurrentDir = RootDir;
        }

        protected void OnCurrentDirChanged(DirectoryInfo inOldRootDir, DirectoryInfo inNewRootDir)
        {
            MELFileSystemFunctionLibrary.PopulateFileSystemInfoList(Files, CurrentDir.FullName);
            ClearSelectedFiles();
        }

        public void Dispose()
        {
            OnCurrentDirChangedDelegate -= OnCurrentDirChanged;
        }
    }
}
