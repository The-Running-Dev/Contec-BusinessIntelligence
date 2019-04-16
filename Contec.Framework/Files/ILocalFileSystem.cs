using System.Collections.Generic;

namespace Contec.Framework.Files
{
    public interface ILocalFileSystem
    {
        long FileLength(string path);

        bool DirectoryExists(string path);

        bool FileExists(string path);

        bool Exists(string path);

        bool FileIsExclusivelyAccessible(string path);

        void Move(string source, string dest, bool allowOverwrite = false);

        void Copy(string source, string dest, bool allowOverwrite = false);

        IEnumerable<string> GetDirectories(string path);

        IEnumerable<string> GetFiles(string path);

        T DeserializeJsonFile<T>(string path);

        T DeserializeJsonFile<T>(string path, string prefix);

        bool SerializeJsonFile<T>(string path, T obj);

        bool SerializeJsonFile<T>(string path, T obj, string prefix);

        T DeserializeXmlFile<T>(string path);

        bool SerializeXmlFile<T>(string path, T obj);

        void DownloadFile(string remoteUrl, string localPath);

        string GetFileContents(string path);

        void WriteToFile(string path, string content);

        bool CreateDirectory(string path);

        bool CreateFile(string path);

        void Delete(string path);

        void DeleteIfOlderThan(string path, int hours);

        void DeleteFilesIfOlderThan(string path, int hours);

        void DeleteSubdirectoriesIfOlderThan(string path, int hours);

        // Utility Methods
        bool IsMedia(string path); 
    }
}