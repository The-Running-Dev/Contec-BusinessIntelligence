namespace Contec.Framework.Extensions
{
    public static class PathExtensions
    {
        public const string AlphaNumericVariableRegEx = @"{{([a-zA-Z0-9\-\._]+)}}";

        public static string VariablesToValue(this string value)
        {
            return value.RegExReplace(AlphaNumericVariableRegEx, "$1");
        }

        public static bool IsUncPath(this string value)
        {
            return value.StartsWith("\\\\");
        }
    }
}