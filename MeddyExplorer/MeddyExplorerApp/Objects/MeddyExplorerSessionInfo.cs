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
        public delegate void StringDelegate(string string1, string string2);
        public event StringDelegate OnCurrentDirChanged;


        public string RootDir { get; set; }
        private string currentDir;
        public string CurrentDir
        {
            get { return currentDir; }
            set 
            {
                string oldCurrentDir = currentDir;

                currentDir = value;

                if (OnCurrentDirChanged is not null)
                {
                    OnCurrentDirChanged.Invoke(oldCurrentDir, currentDir);
                }
            }
        }


        public MeddyExplorerSessionInfo()
        {
            
        }
    }
}
