using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter.TypeTree
{
    public class NamespaceInfo
    {

        public string Namespace;
        public Dictionary<string, TypeInfo> Types;

        public NamespaceInfo()
        {
            this.Types = new Dictionary<string, TypeInfo>();
        }

        public bool ExistsType(string typeName)
        {
            return Types.ContainsKey(typeName);
        }

    }
}
