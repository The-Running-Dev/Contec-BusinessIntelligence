using System;
using System.Collections.Generic;

namespace Contec.Framework.Files
{
    public interface IFileSystem
    {
        long FileLength(FilePath path);

        bool DirectoryExists(FilePath path);

        bool FileExists(FilePath path);

        bool Exists(FilePath path);

        bool FileIsExclusivelyAccessible(FilePath path);

        void Move(FilePath source, FilePath dest, bool allowOverwrite = false);

        void Copy(FilePath source, FilePath destination, bool allowOverwrite = false);

        IEnumerable<string> GetDirectories(FilePath path);

        IEnumerable<string> GetFiles(FilePath path);

        T DeserializeJsonFile<T>(FilePath path);

        T DeserializeJsonFile<T>(FilePath path, string prefix);

        bool SerializeJsonFile<T>(FilePath path, T obj);

        bool SerializeJsonFile<T>(FilePath path, T obj, string prefix);

        T DeserializeXmlFile<T>(FilePath path);

        bool SerializeXmlFile<T>(FilePath path, T obj);
        
        void DownloadFile(string remoteUrl, FilePath localPath);

        string GetFileContents(FilePath path);

        void WriteToFile(FilePath path, string content);

        bool CreateDirectory(FilePath path);

        bool CreateFile(FilePath path);

        void Delete(FilePath path);

        void DeleteIfOlderThan(FilePath path, int hours);

        void DeleteFilesIfOlderThan(FilePath path, int hours);

        void DeleteSubdirectoriesIfOlderThan(FilePath path, int hours);

        // Utility Methods
        bool IsMedia(FilePath path);

        void Execute(FilePath path, Action<string> action);

        void Execute(FilePath source, FilePath destination, Action<string, string> action);

        T Execute<T>(FilePath path, Func<string, T> func);

        T Execute<T>(FilePath source, FilePath destination, Func<string, string, T> action);
    }
}