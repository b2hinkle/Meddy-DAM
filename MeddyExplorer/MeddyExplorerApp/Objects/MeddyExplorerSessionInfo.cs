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
        public delegate void DirectoryParametersDelegate(DirectoryInfo in1, DirectoryInfo in2);
        public event DirectoryParametersDelegate OnCurrentDirChanged;


        public DirectoryInfo RootDir { get; set; }
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
    }
}
