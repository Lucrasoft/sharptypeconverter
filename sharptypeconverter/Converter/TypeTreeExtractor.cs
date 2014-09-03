using Converter.TypeTree;
using ICSharpCode.NRefactory.CSharp;

namespace Converter
{
    class TypeTreeExtractor
    {
        private readonly Tree typeTree;
        public TypeTreeExtractor(Tree typeTree)
        {
            this.typeTree = typeTree;
        }

        public void Process( SyntaxTree syntax)
        {
            foreach (AstNode node in syntax.Children)
            {
                ProcessNode(node);
            }
        }

        private void ProcessNode(AstNode node)
        {
            if ((node.Role.ToString() == "Member") || (node.Role.ToString() == "TypeMember"))
            {
                switch (node.GetType().ToString())
                {
                    case "ICSharpCode.NRefactory.CSharp.NamespaceDeclaration":
                        var nsd = (NamespaceDeclaration)node;
                        typeTree.PushNameSpace(nsd.Name);
                        foreach (AstNode child in node.Children)
                        {
                            ProcessNode(child);
                        }
                        typeTree.PopNameSpace();
                        break;
                    case "ICSharpCode.NRefactory.CSharp.TypeDeclaration":
                        var td = (TypeDeclaration)node;
                        var prevType = typeTree.ActiveType;
                        typeTree.SetActiveType(td.Name, td.ClassType);

                        foreach (var child in node.Children)
                        {
                            ProcessNode(child);
                        }
                        typeTree.SetActiveType(prevType);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.FieldDeclaration":
                        typeTree.ActiveType.AddField((FieldDeclaration)node);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.OperatorDeclaration":
                        var od = (OperatorDeclaration)node;
                        typeTree.ActiveType.AddOperator(od);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.PropertyDeclaration":
                        var pd = (PropertyDeclaration)node;
                        typeTree.ActiveType.AddProperty(pd);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.MethodDeclaration":
                        var md = (MethodDeclaration)node;
                        typeTree.ActiveType.AddMethod(md);
                        break;
                    case "ICSharpCode.NRefactory.CSharp.ConstructorDeclaration":
                        var cd = (ConstructorDeclaration)node;
                        typeTree.ActiveType.AddConstructor(cd);
                        break;
                }
            }
        }
    }
}
