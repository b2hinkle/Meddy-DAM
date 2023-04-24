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
    public class MeddyExplorerState : IDisposable
    {
        public List<FileSystemInfo> Files { get; set; } = new();

        public event Action OnIncludeMeddydataFilesChangedEvent;
        private bool _includeMeddydataFiles;
        public bool IncludeMeddydataFiles
        {
            get
            {
                return _includeMeddydataFiles;
            }
            set
            {
                _includeMeddydataFiles = value;

                if (OnIncludeMeddydataFilesChangedEvent is not null)
                {
                    OnIncludeMeddydataFilesChangedEvent.Invoke();
                }
            }
        }

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

        public event Action<FileSystemInfo> OnFileTargeted;
        public event Action<FileSystemInfo> OnFileUntargeted;

        private FileSystemInfo _targetedFile;
        /// <summary>
        /// The file system item that the user has targeted.
        /// Not necessarily selected.
        /// </summary>
        public FileSystemInfo TargetedFile
        {
            get
            {
                return _targetedFile;
            }
            set
            {
                FileSystemInfo oldValue = _targetedFile;
                _targetedFile = value;

                if (OnFileUntargeted is not null)
                {
                    if (oldValue is not null)
                    {
                        OnFileUntargeted.Invoke(oldValue);
                    }
                }
                if (OnFileTargeted is not null)
                {
                    if (_targetedFile is not null)
                    {
                        OnFileTargeted.Invoke(_targetedFile);
                    }
                }
            }
        }

        public event Action OnSelectedFilesChanged;
        public event Action<FileSystemInfo> OnFileSelected;
        public event Action<FileSystemInfo> OnFileDeselected;

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
                OnFileSelected.Invoke(inFileSystemInfo);
            }
            if (OnSelectedFilesChanged is not null)
            {
                OnSelectedFilesChanged.Invoke();
            }
        }
        public void RemoveFromSelectedFiles(FileSystemInfo inFileSystemInfo)
        {
            SelectedFiles.Remove(inFileSystemInfo);

            if (OnFileDeselected is not null)
            {
                OnFileDeselected.Invoke(inFileSystemInfo);
            }
            if (OnSelectedFilesChanged is not null)
            {
                OnSelectedFilesChanged.Invoke();
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
            OnIncludeMeddydataFilesChangedEvent += OnIncludeMeddydataFilesChanged;
            RootDir = new DirectoryInfo(inRootDir);
            SelectedFiles = new List<FileSystemInfo>();
            App.persistentData.AddNewRecentMeddyProject(RootDir);
            CurrentDir = RootDir;
        }

        public void PopulateFiles()
        {
            Files.Clear();

            string[] subfolderPaths = Directory.GetDirectories(CurrentDir.FullName); // during testing I got an exception "Access to the path D:/Recovery not authorized" or something like that. Maybe we should try/catch
            foreach (string subfolderPath in subfolderPaths)
            {
                Files.Add(new DirectoryInfo(subfolderPath));
            }

            string[] filePaths = Directory.GetFiles(CurrentDir.FullName);
            foreach (string filePath in filePaths)
            {
                if (!IncludeMeddydataFiles)
                {
                    // See if this is a meddydata file
#nullable enable
                    string? CorrespondingFilePath = MeddyFunctionLibrary.GetFilePathFromMeddydataSidecarPath(filePath);
                    if (CorrespondingFilePath != null)
#nullable disable
                    {
                        if (filePaths.Contains(CorrespondingFilePath))
                        {
                            // We are a meddydata file
                            continue;
                        }

                        if (MeddyFunctionLibrary.FilePathsAreEqual(CurrentDir.FullName, CorrespondingFilePath))
                        {
                            // We are a meddydata file
                            continue;
                        }
                    }
                }

                Files.Add(new FileInfo(filePath));
            }
        }

        protected void OnCurrentDirChanged(DirectoryInfo inOldRootDir, DirectoryInfo inNewRootDir)
        {
            PopulateFiles();
            ClearSelectedFiles();
        }

        protected void OnIncludeMeddydataFilesChanged()
        {
            PopulateFiles();
        }

        public void Dispose()
        {
            OnCurrentDirChangedDelegate -= OnCurrentDirChanged;
        }
    }
}
