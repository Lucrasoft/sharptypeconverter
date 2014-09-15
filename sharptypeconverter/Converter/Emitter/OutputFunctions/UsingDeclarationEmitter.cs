using ICSharpCode.NRefactory.CSharp;

namespace Converter.Emitter.OutputFunctions
{
    internal class UsingDeclarationEmitter: BaseEmitter<UsingDeclaration>
    {
        private readonly CodeSegment output;

        internal UsingDeclarationEmitter(EmitterArguments arguments) : base(arguments)
        {
            output = arguments.Output;
        }
        internal override void Output(UsingDeclaration node)
        {
            //registrer the used namespace, to determine TypeReferenceExpressions
            EmitterArguments.ReferencedNamespaces.Add(node.Namespace);
            //if namespace is part of the currenc project scope. skip \library\ 
            if (EmitterArguments.TypeTree.ExistsNamespace(node.Namespace))
            {
                output.Add(string.Format("/// <reference path='{0}.d.ts' />", node.Namespace));
            }
            else
            {
                output.Add(string.Format("/// <reference path='./Library/{0}.d.ts' />", node.Namespace));
            }
        }
    }
}
