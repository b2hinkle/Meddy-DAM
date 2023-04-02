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
        public static void PopulateFileSystemInfoList(List<FileSystemInfo> inFileSystemInfos, string inPath, bool inClearBeforehand = true)
        {
            if (inClearBeforehand)
            {
                inFileSystemInfos.Clear();
            }
            string[] subfolderPaths = Directory.GetDirectories(inPath); // during testing I got an exception "Access to the path D:/Recovery not authorized" or something like that. Maybe we should try/catch
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
