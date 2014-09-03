using ICSharpCode.NRefactory.CSharp;

namespace Converter.Emitter.OutputFunctions
{
    internal class YieldReturnStatementEmitter : BaseEmitter<YieldReturnStatement>
    {
        private readonly CodeSegment output;

        internal YieldReturnStatementEmitter(EmitterArguments arguments)
            : base(arguments)
        {
            output = arguments.Output;
        }
        internal override void Output(YieldReturnStatement node)
        {
            output.Add("yield");
            ExpressionEmitter.Output(node.Expression,EmitterArguments);
            output.Add(";");
        }
    }
}
