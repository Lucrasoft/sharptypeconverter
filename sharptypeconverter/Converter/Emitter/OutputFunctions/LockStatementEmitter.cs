using ICSharpCode.NRefactory.CSharp;

namespace Converter.Emitter.OutputFunctions
{
    internal class LockStatementEmitter : BaseEmitter<LockStatement>
    {
        internal LockStatementEmitter(EmitterArguments arguments) : base(arguments){ }
        internal override void Output(LockStatement node)
        {
            EmitterArguments.Output.Comment("Skipped lock statement :  lock (" + node.Expression + ")");
            Emitter.ProcessNode(node.EmbeddedStatement,EmitterArguments);
        }
    }
}
