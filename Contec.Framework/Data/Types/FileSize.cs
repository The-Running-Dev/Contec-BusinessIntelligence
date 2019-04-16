using System.Collections.Generic;

namespace Contec.Framework.Data.Types
{
    public class FileSize
    {
        public static readonly Dictionary<FileSizeUnit, ulong> Conversions =
            new Dictionary<FileSizeUnit, ulong>() {
                {FileSizeUnit.Bytes, 1},
                {FileSizeUnit.Kilobytes, 1024},
                {FileSizeUnit.Megabytes, 1048576},
                {FileSizeUnit.Gigabytes, 1073741824},
                {FileSizeUnit.Terabytes, 1099511627776}
            };

        public static readonly FileSize MinFileSize = new FileSize();
        public static readonly FileSize MaxFileSize = new FileSize(ulong.MaxValue);
        public static readonly FileSizeUnit DefaultFileSizeUnit = FileSizeUnit.Bytes;
        public static readonly FileSize DefaultFileSize = MinFileSize;

        public FileSize(ulong value = 0, FileSizeUnit unit = FileSizeUnit.Bytes)
        {
            Value = value;
            Unit = unit;
        }

        public ulong Value { get; set; }
        public FileSizeUnit Unit { get; set; }

        public FileSize ConvertTo(FileSizeUnit newUnit)
        {
            //-------------------------------------------------------------------------------------
            // Return the current value if its already in the request units.
            if (newUnit == Unit) return this;

            //-------------------------------------------------------------------------------------
            // Get the number of bytes represented by the current value.
            var bytes = Value * Conversions[Unit];

            //-------------------------------------------------------------------------------------
            // Convert the number of bytes into the requested file size unit.
            var newValue = bytes / Conversions[newUnit];

            return new FileSize(newValue, newUnit);
        }

        public static FileSize operator +(FileSize size1, FileSize size2)
        {
            return new FileSize(size1.Value + size2.ConvertTo(size1.Unit).Value, size1.Unit);
        }

        public static FileSize operator -(FileSize size1, FileSize size2)
        {
            return new FileSize(size1.Value - size2.ConvertTo(size1.Unit).Value, size1.Unit);
        }
    }
}