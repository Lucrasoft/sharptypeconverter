using ICSharpCode.NRefactory.CSharp;
using System.Collections.Generic;
using System.Linq;

namespace Converter.TypeTree
{
    public class Tree
    {
        private readonly Dictionary<string, NamespaceInfo> namespaces;
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
            if (!namespaces.ContainsKey(ActiveNameSpace))
            {
                namespaces.Add(ActiveNameSpace, new NamespaceInfo());
            }
            var nsItem = namespaces[ActiveNameSpace];
            if (!nsItem.Types.ContainsKey(name))
            {
                var newtype = new TypeInfo(name, classType);
                nsItem.Types.Add(name, newtype);
            }
            activeType = nsItem.Types[name];
        }
    }
}
