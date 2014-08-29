using ICSharpCode.NRefactory.CSharp;

namespace Converter.Emitter.OutputFunctions
{
    internal class WhileStatementEmitter : BaseEmitter<WhileStatement>
    {
        private readonly CodeSegment output;

        internal WhileStatementEmitter(EmitterArguments arguments) : base(arguments)
        {
            output = arguments.Output;
        }
        internal override void Output(WhileStatement node)
        {
            output.AddWithoutSpace("while (");
            ExpressionEmitter.Output(node.Condition,EmitterArguments);
            output.Add(") {");
            output.IndentIncrease();

            Emitter.ProcessNode(node.EmbeddedStatement,EmitterArguments);
            output.IndentDecrease();
            output.AddLine("}");
        }
    }
}
