using System;
using System.Configuration;

namespace Contec.Framework.Configuration
{
    [ConfigurationCollection(typeof(IniConfigurationSectionElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public sealed class IniConfigurationSectionCollection : ConfigurationElementCollection
    {
        public IniConfigurationSectionCollection()
            : base()
        {
        }

        public IniConfigurationSectionElement this[int index]
        {
            get { return (IniConfigurationSectionElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null) BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }

        public new IniConfigurationSectionElement this[string name]
        {
            get
            {
                foreach (IniConfigurationSectionElement elem in this)
                {
                    return elem;
                }

                return null;
            }
        }

        public void Add(IniConfigurationSectionElement element)
        {
            BaseAdd((IniConfigurationSectionElement)element);
        }

        public void Clear()
        {
            BaseClear();
        }

        public int IndexOf(IniConfigurationSectionElement element)
        {
            return BaseIndexOf(element);
        }

        public void Remove(IniConfigurationSectionElement element)
        {
            BaseRemove(element.Name);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new IniConfigurationSectionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((IniConfigurationSectionElement)element).Name;
        }

        protected override void BaseAdd(int index, ConfigurationElement element)
        {
            //-------------------------------------------------------------------------------------
            if (!(element is IniConfigurationSectionElement)) base.BaseAdd(element);

            //-------------------------------------------------------------------------------------
            if (IndexOf((IniConfigurationSectionElement)element) >= 0)
            {
                var msg = String.Format("The element {0} already exist!",
                    ((IniConfigurationSectionElement)element).Name);

                throw new ConfigurationErrorsException(msg);
            }

            this[index] = (IniConfigurationSectionElement)element;
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return base.Properties; }
        }
    }
}