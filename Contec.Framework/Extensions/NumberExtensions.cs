using System;

namespace Contec.Framework.Extensions
{
    public static class NumberExtensions
    {
        public static int IterateFromZero(this int maxCount, Action<int> eachAction)
        {
            for (var idx = 0; idx < maxCount; idx++)
            {
                eachAction(idx);
            }

            return maxCount;
        }

        public static int IterateToZero(this int maxCount, Action<int> eachAction)
        {
            for (var idx = maxCount; idx >= 0; idx--)
            {
                eachAction(idx);
            }

            return maxCount;
        }
    }
}
