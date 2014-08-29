using ICSharpCode.NRefactory.CSharp;

namespace Converter.Emitter.OutputFunctions
{
    internal class ThrowStatementEmitter : BaseEmitter<ThrowStatement>
    {
        internal ThrowStatementEmitter(EmitterArguments arguments) : base(arguments) { }
        internal override void Output(ThrowStatement node)
        {
            EmitterArguments.Output.Add("throw");
            ExpressionEmitter.Output(node.Expression, EmitterArguments);
            EmitterArguments.Output.AddLine(";");
        }
    }
}
