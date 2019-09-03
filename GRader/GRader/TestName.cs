using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GRader
{
    class TestName
    {
        private string name;
        public string Name { get => name; }

        public TestName(XmlNode node)
        {
            // test name can be set in Test description
            foreach (XmlNode child in node)
            {
                if (child.Name == "properties")
                {
                    name = getDescription(child);
                    if (name != null) break;
                }
            }

            // if not set, make up a test name by evaluating class name and method name
            if (name == null)
            {
                string className = node.Attributes["classname"].Value;
                string methodName = node.Attributes["methodname"].Value;
                {
                    var split = className.Split(new[] { '.' });
                    if (split.Length > 1)
                    {
                        className = split[1];
                    }
                }
                {
                    var split = methodName.Split(new[] { '_' });
                    if (split.Length > 1) methodName = split[1];
                }
                name = className + " - " + methodName;
            }
        }

        private string getDescription(XmlNode node)
        {
            string description = null;
            foreach (XmlNode child in node)
            {
                if (child.Name == "property" && child.Attributes["name"].Value == "Description")
                {
                    description = child.Attributes["value"].Value;
                }
            }
            return description;
        }
    }
}
