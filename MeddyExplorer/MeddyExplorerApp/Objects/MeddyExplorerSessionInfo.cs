using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeddyExplorerApp.Objects
{
    internal class MeddyExplorerSessionInfo
    {
        public delegate void DirectoryParametersDelegate(DirectoryInfo inOldCurrentDir, DirectoryInfo inNewCurrentDir);
        public event DirectoryParametersDelegate OnCurrentDirChanged;

        public DirectoryInfo RootDir { get; set; }
        private DirectoryInfo currentDir;
        public DirectoryInfo CurrentDir
        {
            get
            {
                return currentDir;
            }
            set
            {
                DirectoryInfo oldValue = currentDir;

                currentDir = value;

                if (OnCurrentDirChanged is not null)
                {
                    OnCurrentDirChanged.Invoke(oldValue, currentDir);
                }
            }
        }
    }
}
