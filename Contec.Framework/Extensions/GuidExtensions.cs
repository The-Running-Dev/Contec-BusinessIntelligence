using System;

namespace Contec.Framework.Extensions
{
    public static class GuidExtensions
    {
        /// <summary>
        /// Checks if a Guid is empty
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static bool IsEmpty(this Guid guid)
        {
            return guid == Guid.Empty;
        }

        /// <summary>
        /// Checks if a Guid is not empty
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static bool IsNotEmpty(this Guid guid)
        {
            return !guid.IsEmpty();   
        }
    }
}
