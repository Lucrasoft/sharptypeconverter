using ICSharpCode.NRefactory.CSharp;

namespace Converter.Emitter.OutputFunctions
{
    internal class ReturnStatementEmitter : BaseEmitter<ReturnStatement>
    {
        internal ReturnStatementEmitter(EmitterArguments arguments) : base(arguments) {}
        internal override void Output(ReturnStatement node)
        {
            EmitterArguments.Output.Add("return");
            ExpressionEmitter.Output(node.Expression,EmitterArguments);
            EmitterArguments.Output.AddLine(";");
        }
    }
}
