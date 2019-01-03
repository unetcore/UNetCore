
    using System;
    using System.Xml;

    public static class XmlExtension
    {
        public static void EachChildren(this XmlNode node, string path, Action<XmlNode> action)
        {
            if (action != null)
            {
                XmlNodeList list = string.IsNullOrEmpty(path) ? node.ChildNodes : node.SelectNodes(path);
                if (list != null)
                {
                    foreach (XmlNode node2 in list)
                    {
                        action(node2);
                    }
                }
            }
        }

        public static string GetAttributeValue(this XmlNode node, string name)
        {
            Guard.ArgumentNull(node, "node", null);
            if (node.Attributes[name] != null)
            {
                return node.Attributes[name].Value;
            }
            return string.Empty;
        }

        public static T GetAttributeValue<T>(this XmlNode node, string name)
        {
            Guard.ArgumentNull(node, "node", null);
            if (node.Attributes[name] != null)
            {
                return node.Attributes[name].Value.To<string, T>(default(T));
            }
            return default(T);
        }

        public static T GetAttributeValue<T>(this XmlNode node, string name, T defaultValue )
        {
            Guard.ArgumentNull(node, "node", null);
            if (node.Attributes[name] != null)
            {
                return node.Attributes[name].Value.To<string, T>(defaultValue);
            }
            return defaultValue;
        }

        public static T GetValue<T>(this XmlAttribute attribute)
        {
            Guard.ArgumentNull(attribute, "attribute", null);
            return attribute.Value.To<string, T>(default(T));
        }

        public static string ToCDATA(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            return ("<![CDATA[" + value + "]]>");
        }
    }
