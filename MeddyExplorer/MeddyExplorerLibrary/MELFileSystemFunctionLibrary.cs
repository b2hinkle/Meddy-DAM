using MudBlazor;
using System;
using static MudBlazor.CategoryTypes;

namespace MeddyExplorerLibrary
{
    /// <summary>
    /// Collection of helpful functions for using the file system
    /// </summary>
    public static class MELFileSystemFunctionLibrary
    {
        public static void PopulateFileSystemInfoList(List<FileSystemInfo> inFileSystemInfos, string inPath)
        {
            string[] subfolderPaths = Directory.GetDirectories(inPath);
            foreach (string subfolderPath in subfolderPaths)
            {
                inFileSystemInfos.Add(new DirectoryInfo(subfolderPath));
            }

            string[] filePaths = Directory.GetFiles(inPath);
            foreach (string filePath in filePaths)
            {
                inFileSystemInfos.Add(new FileInfo(filePath));
            }
        }

        public static string GetFileSystemInfoIcon(FileSystemInfo inFileSystemInfo)
        {
            if (inFileSystemInfo is FileInfo)
            {
                return Icons.Material.Filled.InsertDriveFile;
            }

            if (inFileSystemInfo is DirectoryInfo)
            {
                return Icons.Material.Filled.Folder;
            }

            return string.Empty;
        }

        public static string GetFileSystemInfoNameString(FileSystemInfo inFileSystemInfo)
        {
            return inFileSystemInfo.Name;
        }

        public static string GetFileSystemInfoDateString(FileSystemInfo inFileSystemInfo)
        {
            return inFileSystemInfo.LastWriteTime.ToString("yyyy-MM-dd hh-mm-ss");
        }

        public static string GetFileSystemInfoSizeString(FileSystemInfo inFileSystemInfo)
        {
            FileInfo? fileInfo = inFileSystemInfo as FileInfo;
            if (fileInfo is not null)
            {
                return fileInfo.Length.ToString();
            }

            DirectoryInfo? directoryInfo = inFileSystemInfo as DirectoryInfo;
            if (directoryInfo is not null)
            {
                // Calculation for size of this directory
                string sizeInBytes = string.Empty;
                return sizeInBytes;
            }

            return string.Empty;
        }

        public static string GetFileSystemInfoTypeString(FileSystemInfo inFileSystemInfo)
        {
            FileInfo? fileInfo = inFileSystemInfo as FileInfo;
            if (fileInfo is not null)
            {
                return fileInfo.Extension;
            }

            if (inFileSystemInfo is DirectoryInfo)
            {
                return "Folder";
            }

            return string.Empty;
        }
    }
}
