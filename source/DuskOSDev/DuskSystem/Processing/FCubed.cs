using DuskOSDev.DuskSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Processing
{
    public static class FCubedFormatter
    {
        public static string FormatObject(object o)
        {
            if (o is char)
                return $"'{(char)o}'";
            else if (o is short)
                return $"{(short)o}s";
            else if (o is long)
                return $"{(long)o}l";
            else if (o is int)
                return $"{(int)o}";
            else if (o is ushort)
                return $"({(ushort)o})s";
            else if (o is ulong)
                return $"({(ulong)o})s";
            else if (o is uint)
                return $"({(uint)o})s";
            else if (o is decimal)
                return $"{(decimal)o}";
            else if (o is double)
                return $"{(double)o}d";
            else if (o is float)
                return $"{(float)o}f";
            else if (o is object[])
                return FormatArray((object[])o);
            else if (o is List<object>)
                return FormatList((List<object>)o);
            else if (o is bool)
                return $"{(bool)o}";
            else if (o is string)
                return $"\"{(string)o}\"";
            else if (o is null)
                return "~";
            else throw new InvalidOperationException("Cannot format an unsupported type.");
        }

        public static string FormatArray(object[] obj)
        {
            string s = "";
            foreach (object o in obj)
            {
                if (s.Length > 0)
                    s += ($", {FormatObject(o)}");
                else
                    s = FormatObject(o);
            }
            return $"[{s}]";
        }

        public static string FormatList(List<object> obj)
            => $"!{FormatArray(obj.ToArray())}";

    }

    public class FCubedProperty
    {
        private string key;
        private object value;

        public FCubedProperty(string key, object value)
        {
            if (key.IsNullWhiteSpaceOrEmpty() && !(this is FCubedDocument && this is FCubedPropertyCollection))
                throw new ArgumentNullException("key");
            this.key = key;
            this.value = value;
        }

        public int IndentLength { get; internal set; } = 0;

        public string GetKey() => key;
        public object GetValue() => value;

        internal string GetIndent() => new string(' ', IndentLength);

        public override string ToString()
        {
            if (!(this is FCubedDocument && this is FCubedPropertyCollection))
                return $"{(IndentLength > 0 ? GetIndent() : "")}{GetKey()}={FCubedFormatter.FormatObject(GetValue())}";
            else if (this is FCubedPropertyCollection)
                return (IndentLength > 0 ? GetIndent() : "") + ((FCubedPropertyCollection)this).ToString();
            else if (this is FCubedDocument)
                return (IndentLength > 0 ? GetIndent() : "") + ((FCubedDocument)this).ToString();
            else throw new InvalidOperationException("Cannot create string of unknown type.");
        }
    }

    public class FCubedProperty<T> : FCubedProperty
    {
        public FCubedProperty(string key, T value) : base(key, value) { }

        public new T GetValue() => (T)base.GetValue();
    }

    public class FCubedPropertyCollection : FCubedProperty<List<FCubedProperty>>
    {
        public FCubedPropertyCollection() : base("", new List<FCubedProperty>())
        { }

        public FCubedProperty[] GetProperties()
            => GetValue().ToArray();

        public bool Contains(FCubedProperty property)
            => GetValue().Contains(property);

        public bool Contains(string name)
            => GetProperty(name) != null;

        public FCubedProperty GetProperty(string name)
        {
            foreach (var p in GetProperties())
            {
                if (p.GetKey().EqualsLowerCase(name))
                    return p;
                else continue;
            }
            return null;
        }

        public void Add(FCubedProperty property)
        {
            if (!Contains(property.GetKey()) && !Contains(property))
                GetValue().Add(property);
        }

        public void Remove(FCubedProperty property)
        {
            GetValue().Remove(property);
        }

        public void Remove(string name)
            => Remove(GetProperty(name));

        public int Count => GetValue().Count;

        public int IndexOf(FCubedProperty property)
            => GetValue().IndexOf(property);

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var x in GetValue())
            {
                if (builder.Length > 0)
                    builder.Append($"{Environment.NewLine}{(x.IndentLength > 0 ? GetIndent() : "")}{x}");
                else
                    builder.Append($"{(x.IndentLength > 0 ? GetIndent() : "")}{x}");
            }
            return builder.ToString();
        }
    }

    public sealed class FCubedDocument : FCubedPropertyCollection
    {
        private string documentRoot = "";

        public FCubedDocument(string root) : base() { }

        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            if (!documentRoot.IsNullWhiteSpaceOrEmpty())
            {
                foreach (var x in GetValue())
                {
                    x.IndentLength = IndentLength + 4;
                }
                b.AppendLine($"{documentRoot}:");
            }
            b.Append(base.ToString());
            return b.ToString();
        }
    }
}
