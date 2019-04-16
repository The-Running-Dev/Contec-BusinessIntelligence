using System.IO;

namespace Contec.Framework.Extensions
{
    public static class StreamExtensions
    {
        #region Private Methods

        #region CopyBytes(this Stream dest, byte[] data)

        /// <summary>
        /// Copy the given bytes array into the given stream.
        /// </summary>
        /// <param name="dest">The stream to copy the bytes to.</param>
        /// <param name="data">The bytes to copy into the stream.</param>
        public static void CopyBytes(this Stream dest, byte[] data)
        {
            var ms = new MemoryStream(data);
            ms.Seek(0, SeekOrigin.Begin);

            dest.Seek(0, SeekOrigin.Begin);
            ms.CopyTo(dest);
            dest.Seek(0, SeekOrigin.Begin);
        }

        #endregion

        #region GetBytes(Stream dest)

        /// <summary>
        /// Get the array of bytes currently in the stream.
        /// </summary>
        /// <param name="src">The stream to ge the bytes from.</param>
        /// <returns>The array of bytes currently in the stream.</returns>
        public static byte[] GetBytes(this Stream src)
        {
            var ms = new MemoryStream();
            ms.Seek(0, SeekOrigin.Begin);

            src.Seek(0, SeekOrigin.Begin);
            src.CopyTo(ms);
            src.Seek(0, SeekOrigin.Begin);

            return ms.ToArray();
        }

        #endregion

        #endregion
    }
}
