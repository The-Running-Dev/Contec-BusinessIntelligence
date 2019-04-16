using System;

namespace Contec.Framework.Models
{
    public class IdNamePair<T>
    {
        public T Id;

        public string Name;

        public bool IsDefault;

        public Guid CreatedBy;

        public IdNamePair(T id, string name)
            : this(id, name, false, Guid.Empty)
        {
        }

        public IdNamePair(T id, string name, bool isDefault, Guid createdBy)
        {
            Id = id;
            Name = name;
            IsDefault = isDefault;
            CreatedBy = createdBy;
        }
    }
}