using ICSharpCode.NRefactory.CSharp;

namespace Converter.Emitter.OutputFunctions
{
    internal class ExpressionStatementEmitter: BaseEmitter<ExpressionStatement>
    {
        private readonly CodeSegment output;
        internal ExpressionStatementEmitter(EmitterArguments arguments)
            : base(arguments)
        {
            output = arguments.Output;
        }
        internal override void Output(ExpressionStatement node)
        {
            ExpressionEmitter.Output(node.Expression, EmitterArguments);
            if (!EmitterArguments.IsForStatementInitializerBusy) { output.AddLine(";"); }
        }
    }
}
