/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          System/Registry/Registry.cs
 * PROGRAMMERS:     
 *                  WinMister332/Chris Emberley (cemberley@nerdhub.net)
 */

using System.Collections.Generic;

namespace DuskOS.System.Registry
{
    public class RegistryValue
    {
        private object value = null;

        public RegistryValue(object value)
        {
            this.value = value;
        }

        public object GetValue() => value;
        public T GetValue<T>() => (T)GetValue();

        public override string ToString()
        {
            if (value is string)
                return (string)value;
            else if (value is bool)
                return ((bool)value) ? "true" : "false";
            else if (value is int)
                return ((int)value).ToString();
            else if (value is long)
                return ((long)value).ToString();
            else if (value is short)
                return ((short)value).ToString();
            else if (value is decimal)
                return ((decimal)value).ToString();
            else if (value is float)
                return ((float)value).ToString();
            else if (value is RegistryValue)
                return ((RegistryValue)value).ToString();
            else if (value is RegistryItem)
                return ((RegistryItem)value).GetValue().ToString();
            else return value.GetType().Name;
        }
    }

    public class RegistryItem
    {
        private string name = "";
        private RegistryValue value = null;

        public RegistryItem(string name, RegistryValue value)
        {
            this.name = name;
            this.value = value;
        }

        public RegistryItem(string name, object value)
        {
            this.name = name;
            this.value = new RegistryValue(value);
        }

        public string GetKey() => name;
        public RegistryValue GetValue() => value;
    }

    public class RegistryCollection : List<RegistryItem>
    {
        private string registryName = "";
        public RegistryCollection(string registryName)
        {
            this.registryName = registryName;   
        }

        public void Add(string key, object value)
        {
            if (!(value == null))
                Add(new RegistryItem(key, value));
            else return;
        }

        public bool ContainsKey(string key)
        {
            if (!Utilities.Utilities.IsNullWhiteSpaceOrEmpty(key))
            { 
                foreach (RegistryItem item in this)
                {
                    if (item.GetKey().Equals(key))
                        return true;
                    else continue;
                }
            }
            return false;
        }

        public int IndexOfKey(string key)
        {
            foreach (RegistryItem item in this)
            {
                if (item.GetKey().Equals(key))
                    return IndexOf(item);
                else continue;
            }
            return -1;
        }

        public void Remove(string key)
        {
            if (!ContainsKey(key))
                return;
            else
            {
                var i = IndexOfKey(key);
                RemoveAt(i);
            }
        }

        private RegistryValue GetRegValue(string key)
        {
            if (ContainsKey(key))
                return this[IndexOfKey(key)].GetValue();
            else
                return null;
        }

        public T GetValue<T>(string key)
            => GetRegValue(key).GetValue<T>();

        public RegistryValue this[string key]
        {
            get => GetRegValue(key);
            set
            {
                if (ContainsKey(key))
                {
                    var index = IndexOfKey(key);
                    this[index] = new RegistryItem(key, value);
                }
                else
                    Add(new RegistryItem(key, value));
            }
        }

        public bool HasValue(RegistryValue value)
        {
            foreach (var ri in this)
            {
                if (ri.GetValue().Equals(value) || ri.GetValue().GetValue().Equals(value.GetValue()))
                    return true;
                else continue;
            }
            return false;
        }

        public bool HasValueAt(string key, RegistryValue value)
        {
            var x = this[key];
            if (x != null)
            {
                if (x.Equals(value) || x.GetValue().Equals(value.GetValue()))
                    return true;
            }
            return false;
        }

        public string GetRegistryName() => registryName;

        //TODO: Do ToString() for this class.
    }

    internal sealed class Registry : RegistryCollection
    {
        internal Registry() : base("SysReg") { }
    }
}
