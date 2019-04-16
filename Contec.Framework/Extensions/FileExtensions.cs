using System;
using System.IO;
using System.Web;
using System.Xml;
using System.Linq;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

using Newtonsoft.Json;

using Contec.Framework.Strings;

namespace Contec.Framework.Extensions
{
    public static class Files
    {
        public static string JavaScriptInclude(this string filePath)
        {
            string includeTag;
            var javaScriptFile = filePath.MatchingJavaScriptFile();

            // Return an empty script tag so the browser does not complain
            if (javaScriptFile.IsEmpty())
            {
                includeTag = Constants.EmptyJavaScriptTag;
            }
            else
            {
                includeTag = string.Format(Constants.JavaScriptTag, javaScriptFile, RandomNumber());
            }

            return includeTag;
        }

        /// <summary>
        /// Finds a matching JavaScript file for the given path
        /// </summary>
        /// <param name="filePath">The full file path or the file name of the file</param>
        /// <returns></returns>
        public static string MatchingJavaScriptFile(this string filePath)
        {
            var javaScriptWebPath = string.Empty;
            string relativePathToJavaScriptFiles;

            if (filePath.DirectoryName().EndsWithString("admin"))
            {
                relativePathToJavaScriptFiles = Constants.RelativePathToAdminJavaScriptFiles;
            }
            else if (filePath.DirectoryName().Includes("content"))
            {
                relativePathToJavaScriptFiles = Constants.RelativePathToContentJavaScriptFiles;
            }
            else
            {
                relativePathToJavaScriptFiles = Constants.RelativePathToLibraryJavaScriptFiles;
            }

            string javaScriptFile = string.Format(relativePathToJavaScriptFiles, Path.ChangeExtension(filePath.FileName(), "js"));

            if (File.Exists(javaScriptFile.ResolveFilePath()))
            {
                javaScriptWebPath = javaScriptFile.ResolveUrl();
            }

            return javaScriptWebPath;
        }

        
        public static string ResolveFilePath(this string filePath)
        {
            //return string.Format("{0}\\{1}", Installation.Application.WebApplicationPath, filePath.Replace("~/", string.Empty).Replace("/", "\\"));
            return string.Empty;
        }

        public static bool IsMedia(this string filePath)
        {
            //return filePath.FileExtension().ExistsInList(Installation.Application.AudioVideoExtensions);
            return false;
        }
        
        public static bool IsMediaOrShadowFile(this string filePath)
        {
            //return (filePath.FileExtension().IsEqualTo(Constants.ShadowFileExtension) | filePath.FileExtension().ExistsInList(Installation.Application.AudioVideoExtensions));
            return false;
        }

        public static string UniqueFilePath(this string filePath)
        {
            var chopLength = 35;

            var fileNameWithoutExtension = filePath.FileNameWithoutExtension();
            var safeFileName = Regex.Replace(fileNameWithoutExtension, "[^a-z0-9\\\\-]", string.Empty, RegexOptions.IgnoreCase);
            chopLength = safeFileName.Length < 37 ? safeFileName.Length : chopLength;

            var fileName = safeFileName.Substring(0, chopLength);
            //var safePath = string.Format("{0}\\{1}-{2}{3}", filePath.FileDirectory(), fileName, new ShortGuid(Guid.NewGuid()), filePath.FileExtension());

            //return safePath;
            return string.Empty;
        }

        /// <summary>
        /// Create file with unique name
        /// </summary>
        /// <param name="filePath">The full file path or the file name of the file</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string CreateSafeFilePath(this string filePath)
        {
            var uniqueFilePath = string.Empty;

            try
            {
                uniqueFilePath = filePath.UniqueFilePath();

                //FileHelper.WaitForExclusiveFileAccess(filePath);

                File.Move(filePath, uniqueFilePath);
            }
            catch 
            {
                //ex.Message.LogError(ex); 
            }

            return File.Exists(uniqueFilePath) ? uniqueFilePath : filePath;
        }

        /// <summary>
        /// Checks if the file specified exits on the file system
        /// </summary>
        /// <param name="filePath">The full file path or the file name of the file</param>
        /// <param name="directoryPath">Optional directory path where the file is located</param>
        /// <returns>True or false for the existence of the file</returns>
        /// <remarks></remarks>
        public static bool FileExists(this string filePath, string directoryPath = "")
        {
            // If directory path was passed in
            if (!string.IsNullOrEmpty(directoryPath))
            {
                // Else, pre-pend the directory path
                filePath = Path.Combine(directoryPath, Path.GetFileName(filePath));
            }

            return File.Exists(filePath);
        }

        /// <summary>
        /// Checks if exclusive access to a file can be obtained
        /// </summary>
        /// <param name="filePath">The file path of the file</param>
        /// <returns>True or false for the exclusive access to the file</returns>
        /// <remarks></remarks>
        public static bool FileIsExclusivelyAccessible(this string filePath)
        {
            //return new FileStatusChecker(filePath).CheckExclusiveStatus();
            return true;
        }

        /// <summary>
        /// Checks if the file specified is accessible by check if it exists
        /// and if it can be accessed exclusively
        /// </summary>
        /// <param name="fileName">The full file path or the file name of the file</param>
        /// <param name="filePath">A variable to store the full path to the file</param>
        /// <param name="directoryPath">Optional directory path where the file is located</param>
        /// <returns>True or false for the accessibility of the file</returns>
        /// <remarks></remarks>
        public static bool FileIsAccessible(this string fileName, string filePath, string directoryPath = "")
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                filePath = fileName;

                if (filePath.FileExists(directoryPath))
                {
                    // If the file cannot be opened exclusively
                    if (!filePath.FileIsExclusivelyAccessible())
                    {
                        filePath = string.Empty;
                    }
                }
                else
                {
                    filePath = string.Empty;
                }
            }

            return !string.IsNullOrEmpty(filePath);
        }

        //public static MPlayer.MediaProperties GetMediaProperties(this string filePath, bool cleanUp = true)
        //{
        //    MPlayer.Wrapper mPlayer = new MPlayer.Wrapper(Installation.Application.MPlayerPath);

        //    mPlayer.OnErrorOccurred += OnMPlayerError;
        //    mPlayer.OnInfoOccured += OnMPlayerInfo;

        //    // If this is an audio file, always clean up after MPlayer
        //    if (filePath.FileExtension().ExistsInList(Installation.Application.AudioExtensions))
        //    {
        //        cleanUp = true;
        //    }

        //    return mPlayer.GetProperties(filePath, cleanUp);
        //}

        //public static List<MPlayer.Thumbnail> CaptureThumbnails(this string filePath, MPlayer.MediaProperties mediaProperties = null)
        //{
        //    MPlayer.Wrapper mPlayer = new MPlayer.Wrapper(Installation.Application.MPlayerPath);
        //    mPlayer.NumberOfFramesToCapture = 10;

        //    //mPlayer.OnErrorOccurred += OnMPlayerError;
        //    //mPlayer.OnInfoOccured += OnMPlayerInfo;

        //    return mPlayer.CaptureToByteArray(filePath, mediaProperties);
        //}

        //private static void OnMPlayerError(this string logMessage)
        //{
        //    logMessage.LogError();
        //}

        //private static void OnMPlayerInfo(this string logMessage)
        //{
        //    logMessage.LogInfo();
        //}

        /// <summary>
        /// Checks if the namespace of the XML file specified
        /// matches the provided namespace
        /// </summary>
        /// <param name="filePath">The path to the XML file</param>
        /// <param name="xmlNameSpace"></param>
        /// <returns>True or false for match of the namespace</returns>
        /// <remarks></remarks>
        public static bool XmlNamespaceIsEqualTo(this string filePath, string xmlNameSpace)
        {
            var xmlDoc = new XmlDocument();
            var fileXmlNameSpace = string.Empty;

            try
            {
                xmlDoc.Load(filePath);
                fileXmlNameSpace = xmlDoc.DocumentElement.NamespaceURI;
            }
            catch {}

            return fileXmlNameSpace.IsEqualTo(xmlNameSpace);
        }

        /// <summary>
        /// Checks if the namespace of the XML file specified
        /// matches the provided namespace
        /// </summary>
        /// <param name="filePath">The path to the XML file</param>
        /// <returns>True or false for match of the namespace</returns>
        /// <remarks></remarks>
        public static bool RootXmlNodeIsEqualTo(this string filePath, string xmlRootNode)
        {
            var xmlDoc = new XmlDocument();
            var fileXmlRootNode = string.Empty;

            try
            {
                xmlDoc.Load(filePath);
                fileXmlRootNode = xmlDoc.DocumentElement.Name;
            }
            catch { }

            return fileXmlRootNode.IsEqualTo(xmlRootNode);
        }

        public static bool SerializeXmlFile<T>(this string filePath, T schema)
        {
            TextWriter writer = null;

            try
            {
                // Read the xml file
                writer = new StreamWriter(filePath);

                // Get an instance of the schema
                var serializer = new XmlSerializer(typeof (T));

                // Add an empty namespace and empty value
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                // Create an instance of the schema based on the xml file
                serializer.Serialize(writer, schema, ns);
            }
            catch
            {
                return false;
            }
            finally
            {
                writer.Close();
            }

            return true;
        }

        public static T DeserializeXmlFile<T>(this string filePath)
        {
            TextReader reader = null;

            T schema = default(T);

            try
            {
                // Read the xml file
                reader = new StreamReader(filePath);

                // Get an instance of the schema
                var serializer = new XmlSerializer(typeof(T));

                // Create an instance of the schema based on the xml file
                schema = (T)serializer.Deserialize(reader);
            }
            catch { }
            finally
            {
                reader.Close();
            }

            return schema;
        }

        
        public static bool SerializeJsonFile<T>(this string filePath, T jsonObject)
        {
            return filePath.SerializeJsonFile(jsonObject, string.Empty);
        }

        /// <summary>
        /// Creates an file with the serialized contents of the
        /// jsonObject class
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <param name="jsonObject">The object to be serialized</param>
        /// <remarks></remarks>
        public static bool SerializeJsonFile<T>(this string filePath, T jsonObject, string jsonPrefix)
        {
            var createSuccess = false;

            try
            {
                if (jsonPrefix.IsNotEmpty())
                {
                    File.WriteAllText(filePath, string.Format("{0}{1};", jsonPrefix, jsonObject.ToJson()));
                }
                else
                {
                    File.WriteAllText(filePath, jsonObject.ToJson());
                }

                createSuccess = filePath.Exists();
            }
            catch 
            {                 
                //ex.Message.LogError(ex);
            }

            return createSuccess;
        }

        public static T DeserializeJsonFile<T>(this string filePath)
        {
            return filePath.DeserializeJsonFile<T>(string.Empty);
        }
        
        public static T DeserializeJsonFile<T>(this string filePath, string jsonPrefix)
        {
            var returnValue = default(T);

            try
            {
                var fileContents = filePath.JsonFileContents(jsonPrefix);

                returnValue = JsonConvert.DeserializeObject<T>(fileContents);
            }
            catch 
            {
                //ex.Message.LogError(ex);
            }

            return returnValue;
        }

        public static string DirectoryName(this string entryPath)
        {
            return Path.GetDirectoryName(entryPath);
        }

        public static string FileName(this string filePath)
        {
            return Path.GetFileName(filePath);
        }
        
        public static string FileNameWithoutExtension(this string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }
        
        public static string FileExtension(this string filePath)
        {
            return Path.GetExtension(filePath);
        }

        public static void ChangeExtension(this string filePath, string fileExtension)
        {
            try
            {
                if (filePath.Exists())
                {
                    var newFile = string.Format("{0}\\{1}{2}", filePath.FileDirectory(), filePath.FileNameWithoutExtension(), fileExtension);

                    File.Move(filePath, newFile);
                }
            }
            catch
            {
            }
        }
        
        public static void Transfer(this string relativePath)
        {
            HttpContext.Current.Server.Transfer(relativePath);
        }
        
        public static void Redirect(this string relativePath)
        {
            HttpContext.Current.Response.Redirect(relativePath, false);
        }
        
        public static void RedirectAndEndRequest(this string relativePath)
        {
            HttpContext.Current.Response.Redirect(relativePath, true);
        }

        public static void Redirect(this string relativePath, string queryString)
        {
            HttpContext.Current.Response.Redirect(string.Format("{0}?{1}", relativePath, queryString));
        }

        public static void CreateFileOnWebServer(this string fileSystemPath)
        {
            File.CreateText(HttpContext.Current.Server.MapPath(fileSystemPath)).Close();
        }

        public static string FileDirectory(this string filePath)
        {
            return Path.GetDirectoryName(filePath);
        }

        /// <summary>
        /// Checks if the passed in path (directory or file) exists
        /// </summary>
        /// <param name="fileSystemPath">The path to the entry on the file system</param>
        /// <returns>True or false for existance of the file system entry</returns>
        /// <remarks></remarks>
        public static bool Exists(this string fileSystemPath)
        {
            var isDirectory = Directory.Exists(fileSystemPath);
            var isFile = File.Exists(fileSystemPath);

            return isDirectory | isFile;
        }

        /// <summary>
        /// Checks if the passed in directory contains any files
        /// that match the specified filter
        /// </summary>
        /// <param name="directoryPath">The path to the directory on the file system</param>
        /// <param name="fileFilter">The file filter to use</param>
        /// <returns>True or false for existance of any files</returns>
        /// <remarks></remarks>
        public static bool FilesExist(this string directoryPath, string fileFilter)
        {
            var filesFound = false;

            try
            {
                filesFound = Directory.GetFiles(directoryPath, fileFilter).Any();
            }
            catch 
            {
            }

            return filesFound;
        }

        /// <summary>
        /// Deletes the file system entry (directory or file) if it exists
        /// </summary>
        /// <param name="fileSystemPath">The path to the entry on the file system</param>
        /// <remarks></remarks>
        public static void Delete(this string fileSystemPath)
        {
            var isDirectory = Directory.Exists(fileSystemPath);
            var isFile = File.Exists(fileSystemPath);

            try
            {
                if (isDirectory)
                {
                    Directory.Delete(fileSystemPath, true);
                }
                else if (isFile)
                {
                    //FileHelper.WaitForExclusiveFileAccess(fileSystemPath);

                    File.Delete(fileSystemPath);
                }
            }
            catch 
            {
                //ex.Message.LogError(ex);
            }
        }

        /// <summary>
        /// Deletes the file system entry (directory or file) if it exists
        /// </summary>
        /// <param name="fileSystemPath">The path to the entry on the file system</param>
        /// <remarks></remarks>
        
        public static void DeleteIfOlderThan(this string fileSystemPath, int storageHours)
        {
            var isDirectory = Directory.Exists(fileSystemPath);
            var isFile = File.Exists(fileSystemPath);

            try
            {
                DateTime lastModified;

                if (isDirectory)
                {
                    lastModified = new DirectoryInfo(fileSystemPath).LastWriteTime;

                    if (DateTime.Compare(DateTime.Now, lastModified.AddHours(storageHours)) > 0)
                    {
                        //string.Format("Directory \"{0}\" is older than {1} hours...deleting", fileSystemPath, storageHours).LogDebug();

                        Directory.Delete(fileSystemPath, true);
                    }
                }
                else if (isFile)
                {
                    lastModified = new FileInfo(fileSystemPath).LastWriteTime;

                    if (DateTime.Compare(DateTime.Now, lastModified.AddHours(storageHours)) > 0)
                    {
                        //string.Format("File \"{0}\" is older than {1} hours...deleting", fileSystemPath, storageHours).LogDebug();

                        //FileHelper.WaitForExclusiveFileAccess(fileSystemPath);

                        File.Delete(fileSystemPath);
                    }
                }
            }
            catch 
            {
                //ex.Message.LogError(ex);
            }
        }

        /// <summary>
        /// Creates an empty file on the file system
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <remarks></remarks>
        public static bool CreateFile(this string filePath)
        {
            return filePath.Create(true, false);
        }

        /// <summary>
        /// Creates a directory on the file system
        /// </summary>
        /// <param name="directoryPath">The path to the directory</param>
        /// <remarks></remarks>
        public static bool CreateDirectory(this string directoryPath)
        {
            return directoryPath.Create(false, true);
        }

        /// <summary>
        /// Creates the file system entry (directory or file)
        /// </summary>
        /// <param name="fileSystemPath">The path to the entry on the file system</param>
        /// <remarks></remarks>
        public static bool Create(this string fileSystemPath, bool isFile, bool isDirectory)
        {
            var createSuccess = false;

            try
            {
                if (isDirectory)
                {
                    Directory.CreateDirectory(fileSystemPath);
                }
                else if (isFile & !File.Exists(fileSystemPath))
                {
                    Directory.CreateDirectory(fileSystemPath.DirectoryName());
                    File.CreateText(fileSystemPath).Close();
                }

                createSuccess = fileSystemPath.Exists();
            }
            catch { }

            return createSuccess;
        }

        public static bool ExistsOnWebServer(this string fileSystemPath)
        {
            var isDirectory = Directory.Exists(HttpContext.Current.Server.MapPath(fileSystemPath));
            var isFile = File.Exists(HttpContext.Current.Server.MapPath(fileSystemPath));

            return isDirectory | isFile;
        }
        
        public static string GetWebServerPath(this string relativePath)
        {
            string webServerPath = string.Empty;

            if ((HttpContext.Current != null) & relativePath.StartsWith("~/"))
            {
                webServerPath = HttpContext.Current.Server.MapPath(relativePath);
            }
            else
            {
                //webServerPath = string.Format("{0}{1}", Installation.Application.WebApplicationPath, relativePath.Replace("~/", "/").Replace("/", "\\"));
            }

            return webServerPath;
        }

        public static long Size(this string fileSystemPath)
        {
            var isDirectory = Directory.Exists(fileSystemPath);
            var isFile = File.Exists(fileSystemPath);
            long fileSystemEntrySize = 0;

            try
            {

                if (isDirectory)
                {
                }
                else if (isFile)
                {
                    fileSystemEntrySize = new FileInfo(fileSystemPath).Length;
                }
            }
            catch { }

            return fileSystemEntrySize;
        }

        /// <summary>
        /// Reads the text contents of a file
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <remarks></remarks>
        
        public static string FileContents(this string filePath)
        {
            var fileText = string.Empty;

            try
            {
                if (File.Exists(filePath))
                {
                    fileText = File.ReadAllText(filePath);
                }
            }
            catch {}

            return fileText;
        }

        public static string JsonFileContents(this string filePath)
        {
            return filePath.JsonFileContents(string.Empty);
        }

        /// <summary>
        /// Reads the text contents of a file
        /// and strips any JSON prefix and JavaScript comments
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <param name="jsonPrefix">The prefix of the JSON data</param>
        /// <remarks></remarks>
        public static string JsonFileContents(this string filePath, string jsonPrefix)
        {
            var fileText = string.Empty;

            if (filePath.Exists())
            {
                var lineCounter = 0;
                var reader = new StreamReader(filePath);

                try
                {
                    string line;

                    do
                    {
                        line = reader.ReadLine();

                        if ((line != null))
                        {
                            // If the JSON prefix exists
                            if (jsonPrefix.IsNotEmpty())
                            {
                                if (lineCounter == 0)
                                {
                                    // Remove the JSON prefix from the line
                                    line = line.RegExReplace(jsonPrefix.Replace(" ", "\\s?"), string.Empty);
                                }

                                if (line.Trim().EndsWith("};"))
                                {
                                    // Remove the ; from the line
                                    line = line.Replace("};", "}");
                                }
                            }

                            // Ignore lines that are JavaScript comments
                            if (!line.Trim().StartsWith("//"))
                            {
                                fileText += line + Environment.NewLine;
                            }
                        }

                        lineCounter = lineCounter + 1;
                    } while (line != null);
                }
                catch {}
                finally
                {
                    reader.Close();
                }
            }

            return fileText;
        }

        public static int RandomNumber()
        {
            const int minValue = 100;
            const int maxValue = 100000;

            lock (syncLock)
            {
                return getrandom.Next(minValue, maxValue);
            }
        }

        public static void AppendToFile(this string filePath, string textToAppend)
        {
            File.AppendAllText(filePath, textToAppend);
        }

        private static readonly Random getrandom = new Random();
        private static readonly object syncLock = new object();
    }
}