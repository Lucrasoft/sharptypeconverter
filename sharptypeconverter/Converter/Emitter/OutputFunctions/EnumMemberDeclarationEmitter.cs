using ICSharpCode.NRefactory.CSharp;

namespace Converter.Emitter.OutputFunctions
{
    class EnumMemberDeclarationEmitter: BaseEmitter<EnumMemberDeclaration>
    {
        CodeSegment output;

        internal EnumMemberDeclarationEmitter(EmitterArguments arguments)
            : base(arguments)
        {
            output = arguments.Output;
        }
        internal override void Output(EnumMemberDeclaration node)
        {
            output.AddWithoutSpace(node.Name);
            if (!node.Initializer.IsNull)
            {
                output.AddWithoutSpace("=");
                ExpressionEmitter.Output(node.Initializer,EmitterArguments);
            }
        }
    }
}
