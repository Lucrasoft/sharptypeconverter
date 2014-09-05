using ICSharpCode.NRefactory.CSharp;
using System.Collections.Generic;
using System.Linq;

namespace Converter.TypeTree
{
    public class Tree
    {
        private readonly Dictionary<string, NamespaceInfo> namespaces;
        public NamespaceInfo DefaultNamespace { private set; get; }
        public string ActiveNameSpace { private set; get; }
        private TypeInfo activeType;
        public TypeInfo ActiveType
        {
            get
            {
                return activeType;
            }
        }
        private readonly Stack<string> nameSpaceStack;
        public Tree()
        {
            nameSpaceStack = new Stack<string>();
            namespaces = new Dictionary<string, NamespaceInfo>();
            DefaultNamespace = new NamespaceInfo{Namespace = "DefaultNamespace"};
        }

        private void RecalcActiveNameSpace()
        {
            ActiveNameSpace = "";
            if (nameSpaceStack.Count > 0)
            {
                ActiveNameSpace += nameSpaceStack.ElementAt(0);
                for (int i = 1; i < nameSpaceStack.Count; i++)
                {
                    ActiveNameSpace += "." + nameSpaceStack.ElementAt(i);
                }
            }
        }
        public void PushNameSpace(string name)
        {
            nameSpaceStack.Push(name);
            RecalcActiveNameSpace();
        }

        public void PopNameSpace()
        {
            nameSpaceStack.Pop();
            RecalcActiveNameSpace();
        }

        public bool ExistsNamespace(string name)
        {
            return (nameSpaceStack.Count(x => x == name) > 0);
        }

        public bool ExistsTypeInNamespace(string typeName, string nameSpaceName)
        {
            if (nameSpaceName == null)
            {
                return false;
            }
            if (namespaces.ContainsKey(nameSpaceName))
            {
                var ns = namespaces[nameSpaceName];
                return ns.ExistsType(typeName);
            }
            return false;
        }

        public void ClearActiveType()
        {
            activeType = null;
        }

        public void SetActiveType(TypeInfo type)
        {
            activeType = type;
        }

        public void SetActiveType(string name, ClassType classType)
        {
            NamespaceInfo nsItem;
            if (string.IsNullOrEmpty(ActiveNameSpace))
            {
                nsItem = DefaultNamespace;
            }
            else
            {
                if (!namespaces.ContainsKey(ActiveNameSpace))
                {
                    namespaces.Add(ActiveNameSpace, new NamespaceInfo());
                }
                nsItem = namespaces[ActiveNameSpace];
            }
            if (!nsItem.Types.ContainsKey(name))
            {
                var newtype = new TypeInfo(name, classType);
                nsItem.Types.Add(name, newtype);
            }
            activeType = nsItem.Types[name];
        }
    }
}
