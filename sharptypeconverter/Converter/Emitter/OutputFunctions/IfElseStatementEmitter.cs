
using ICSharpCode.NRefactory.CSharp;

namespace Converter.Emitter.OutputFunctions
{
    internal class IfElseStatementEmitter: BaseEmitter<IfElseStatement>
    {
        private readonly CodeSegment output;

        internal IfElseStatementEmitter(EmitterArguments arguments) : base(arguments)
        {
            output = arguments.Output;
        }

        internal override void Output(IfElseStatement node)
        {
            output.Add("if");
            output.AddWithoutSpace("(");
            ExpressionEmitter.Output(node.Condition,EmitterArguments);
            output.Add(") {");
            output.IndentIncrease();
            Emitter.ProcessNode(node.TrueStatement,EmitterArguments);
            output.IndentDecrease();
            output.Add("}");

            if (!node.FalseStatement.IsNull)
            {
                output.Add("else");
                Emitter.ProcessNode(node.FalseStatement,EmitterArguments);
            }
        }
    }
}
