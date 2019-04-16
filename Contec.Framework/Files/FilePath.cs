using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Contec.Framework.Files
{
    public class FilePath
    {
        public string Path { get; set; }

        public string ParentPath { get; set; }

        public FilePathType PathType { get; set; }

        public string Domain { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public FilePath() { }

        public FilePath(string path, string username = "", string password = "", string domain = "")
        {
            Path = path;
            PathType = GetPathType(path);
            Domain = domain;
            UserName = username;
            Password = password;
        }

        public static FilePath New(string path, string username = "", string password = "", string domain = "")
        {
            return new FilePath()
            {
                Path = path,
                PathType = GetPathType(path),
                Domain = domain,
                UserName = username,
                Password = password
            };
        }

        public static FilePath Combine(FilePath original, params string[] paths)
        {
            var allPaths = new List<string>(paths);
            string combinedPath;

            allPaths.Insert(0, original.Path);

            switch (original.PathType)
            {
                case FilePathType.Ftp:
                case FilePathType.Http:
                    combinedPath = string.Join("/", allPaths.ToArray());

                    break;
                default:
                    combinedPath = System.IO.Path.Combine(allPaths.ToArray());

                    break;
            }

            return new FilePath()
            {
                Path = combinedPath,
                ParentPath = original.ParentPath,
                PathType = original.PathType,
                Domain = original.Domain,
                UserName = original.UserName,
                Password = original.Password
            };
        }

        public static FilePathType GetPathType(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return FilePathType.Unknown;
            }

            if (Regex.IsMatch(path, "^\\w:"))
            {
                return FilePathType.Local;
            }

            if (path.StartsWith("\\\\"))
            {
                return FilePathType.Unc;
            }

            if (path.StartsWith("ftp://", StringComparison.CurrentCultureIgnoreCase))
            {
                return FilePathType.Ftp;
            }

            if (path.StartsWith("http", StringComparison.CurrentCultureIgnoreCase))
            {
                return FilePathType.Http;
            }

            if (Regex.IsMatch(path, "^\\w+://"))
            {
                return FilePathType.Unknown;
            }

            return FilePathType.Local;
        }

        public static implicit operator FilePath(string path)
        {
            return New(path);
        }
    }
}