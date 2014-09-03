using ICSharpCode.NRefactory.CSharp;

namespace Converter.Emitter.OutputFunctions
{
    internal class UnsafeStatementEmitter : BaseEmitter<UnsafeStatement>
    {
        internal UnsafeStatementEmitter(EmitterArguments arguments) : base(arguments) { }
        internal override void Output(UnsafeStatement node)
        {
            EmitterArguments.Output.Comment("unsafe statement not supported by Typescript.");
            Emitter.ProcessNode(node.Body,EmitterArguments);
        }
    }
}

