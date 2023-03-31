namespace MeddyExplorerLibrary
{
    /// <summary>
    /// Collection of helpful functions for using the file system
    /// </summary>
    public static class MELFileSystemFunctionLibrary
    {
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
                string sizeInBytes = "";
                return sizeInBytes;
            }

            return "";
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

            return "";
        }
    }
}
