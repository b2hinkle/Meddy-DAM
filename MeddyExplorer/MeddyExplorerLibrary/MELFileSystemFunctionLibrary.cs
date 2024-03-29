﻿using System.Collections.ObjectModel;
using System.Text;
using System.Xml.Serialization;
﻿using MudBlazor;

namespace MeddyExplorerLibrary
{
    /// <summary>
    /// Collection of helpful functions for using the file system
    /// </summary>
    public static class MELFileSystemFunctionLibrary
    {
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
        public static string GetFileSystemInfoIcon(ReadOnlyCollection<FileSystemInfo> inFileSystemInfos)
        {
            if (inFileSystemInfos.Any())
            {
                return GetFileSystemInfoIcon(inFileSystemInfos.Last());
            }

            return string.Empty;
        }

        public static string GetFileSystemInfoNameString(FileSystemInfo inFileSystemInfo)
        {
            return inFileSystemInfo.Name;
        }
        public static string GetFileSystemInfoNameString(ReadOnlyCollection<FileSystemInfo> inFileSystemInfos)
        {
            if (inFileSystemInfos.Any())
            {
                return GetFileSystemInfoNameString(inFileSystemInfos.Last());
            }

            return string.Empty;
        }

        public static string GetFileSystemInfoDateString(FileSystemInfo inFileSystemInfo)
        {
            return inFileSystemInfo.LastWriteTime.ToString("yyyy-MM-dd hh:mm:ss");
        }
        public static string GetFileSystemInfoDateString(ReadOnlyCollection<FileSystemInfo> inFileSystemInfos)
        {
            if (inFileSystemInfos.Any())
            {
                return GetFileSystemInfoDateString(inFileSystemInfos.Last());
            }

            return string.Empty;
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
        public static string GetFileSystemInfoSizeString(ReadOnlyCollection<FileSystemInfo> inFileSystemInfos)
        {
            if (inFileSystemInfos.Any())
            {
                return GetFileSystemInfoSizeString(inFileSystemInfos.Last());
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
        public static string GetFileSystemInfoTypeString(ReadOnlyCollection<FileSystemInfo> inFileSystemInfos)
        {
            if (inFileSystemInfos.Any())
            {
                return GetFileSystemInfoTypeString(inFileSystemInfos.Last());
            }

            return string.Empty;
        }

        public static async Task SaveAsync<TypeToSave>(string inFullFileName, TypeToSave objectToSave)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TypeToSave));

            // Serialize the data to a string using a StringWriter
            StringWriter stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, objectToSave);

            byte[] data = Encoding.UTF8.GetBytes(stringWriter.ToString()); // convert the string to a byte array
            using (FileStream fileStream = new FileStream(inFullFileName, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
            {
                await fileStream.WriteAsync(data, 0, data.Length); // write the data to the file asynchronously
            }
        }
        public static void Save<TypeToSave>(string inFullFileName, TypeToSave objectToSave)
        {
            // Open a file stream to write the XML data to the file
            using (FileStream fileStream = new FileStream(inFullFileName, FileMode.Create))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(TypeToSave)); // create an XmlSerializer to serialize the struct
                xmlSerializer.Serialize(fileStream, objectToSave); // serialize the struct to the file stream
            }
        }
        public static TypeToLoad Load<TypeToLoad>(string inFullFileName)
        {
            if (File.Exists(inFullFileName))
            {
                // Open a file stream to read the XML data from the file
                using (FileStream fileStream = new FileStream(inFullFileName, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(TypeToLoad)); // create an XmlSerializer to deserialize the struct
                    return (TypeToLoad)serializer.Deserialize(fileStream); // deserialize the struct from the file stream
                }
            }
            return default(TypeToLoad);
        }
        public static async Task<TypeToLoad> LoadAsync<TypeToLoad>(string inFullFileName)
        {
            // Read all the data from the file asynchronously
            byte[] data = await File.ReadAllBytesAsync(inFullFileName);

            // Open a file stream to read the XML data from the file
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TypeToLoad)); // create an XmlSerializer to deserialize the struct
                return (TypeToLoad)serializer.Deserialize(memoryStream); // deserialize the struct from the memory stream
            }
            return default(TypeToLoad);
        }
    }
}
