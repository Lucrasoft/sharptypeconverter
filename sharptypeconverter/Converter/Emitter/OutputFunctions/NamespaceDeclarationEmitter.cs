using ICSharpCode.NRefactory.CSharp;
using Converter.TypeTree;


namespace Converter.Emitter.OutputFunctions
{
    internal class NamespaceDeclarationEmitter: BaseEmitter<NamespaceDeclaration>
    {
        readonly CodeSegment output;
        readonly Tree typeTree;
        internal NamespaceDeclarationEmitter(EmitterArguments arguments)
            : base(arguments)
        {
            output = arguments.Output;
            typeTree = arguments.TypeTree;
        }
        internal override void Output(NamespaceDeclaration node)
        {
            typeTree.PushNameSpace(node.Name);
            //TODO : In options 
            output.Add(string.Format("module {0} ", node.Name));
            output.Add("{");
            output.IndentIncrease();
            foreach (AstNode child in node.Children)
            {
                Emitter.ProcessNode(child,EmitterArguments);
            }
            output.IndentDecrease();
            output.Add("}");
            typeTree.PopNameSpace();
        }
    }
}
