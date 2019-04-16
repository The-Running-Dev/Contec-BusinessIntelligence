using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Contec.Framework.Configuration
{
    public class IniConfigurationSettingsCollection : Collection<IniConfigurationSettings>
    {
        private readonly Dictionary<string, int> _lookup = new Dictionary<string, int>();

        public virtual IniConfigurationSettings this[string sectionName]
        {
            get
            {
                if (_lookup.ContainsKey(sectionName))
                {
                    return this[_lookup[sectionName]];
                }

                return null;
            }
        }

        public virtual string this[string sectionName, string fieldName]
        {
            get
            {
                var section = this[sectionName];

                if (section != null)
                {
                    if (section.ContainsKey(fieldName))
                    {
                        return section[fieldName];
                    }
                }

                return null;
            }
            set
            {
                var section = this[sectionName];

                if (section == null)
                {
                    section = new IniConfigurationSettings(sectionName);
                }

                section[fieldName] = value;
            }
        }

        public string Get(string sectionName, string fieldName, string defValue)
        {
            var value = defValue;
            var section = this[sectionName];

            if (section != null)
            {
                if (section.ContainsKey(fieldName))
                {
                    return section[fieldName];
                }
            }

            return value;
        }

        protected override void InsertItem(int index, IniConfigurationSettings item)
        {
            //-------------------------------------------------------------------------------------
            // Add the new item's information to the lookup table.
            _lookup[item.Name] = index;

            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, IniConfigurationSettings item)
        {
            //-------------------------------------------------------------------------------------
            // Add the new item's information to the lookup table.
            _lookup[item.Name] = index;

            base.SetItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            //-------------------------------------------------------------------------------------
            // Remove the item's information from the lookup table.
            _lookup.Remove(this[index].Name);

            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            //-------------------------------------------------------------------------------------
            // Remove all of information in the lookup table.
            _lookup.Clear();

            base.ClearItems();
        }
    }
}