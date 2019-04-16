using System.ComponentModel;

namespace Contec.Framework.Data.Types
{
    public enum FileSizeUnit
    {
        [Description("B")]
        Bytes,
        [Description("KB")]
        Kilobytes,
        [Description("MB")]
        Megabytes,
        [Description("GB")]
        Gigabytes,
        [Description("TB")]
        Terabytes
    }
}