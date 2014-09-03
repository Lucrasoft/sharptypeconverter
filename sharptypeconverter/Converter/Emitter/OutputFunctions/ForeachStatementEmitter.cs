using ICSharpCode.NRefactory.CSharp;

namespace Converter.Emitter.OutputFunctions
{
    internal class ForeachStatementEmitter : BaseEmitter<ForeachStatement>
    {
        private readonly CodeSegment output;

        internal ForeachStatementEmitter(EmitterArguments arguments)
            : base(arguments)
        {
            output = arguments.Output;
        }

        internal override void Output(ForeachStatement node)
        {
            //C# : foreach (var item in collection) {  ...  }
            //TS :  $TS.forEach( collection, function(item) { ... }); 
            output.AddWithoutSpace("$TS.forEach(");
            ExpressionEmitter.Output(node.InExpression, EmitterArguments);
            output.Add(", function(" + node.VariableName + ") {");
            output.IndentIncrease();

            Emitter.ProcessNode(node.EmbeddedStatement, EmitterArguments);
            output.IndentDecrease();
            output.AddLine(" }); ");
        }
    }
}
