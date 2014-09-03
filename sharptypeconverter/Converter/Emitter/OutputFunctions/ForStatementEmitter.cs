using System.Linq;
using ICSharpCode.NRefactory.CSharp;

namespace Converter.Emitter.OutputFunctions
{
    internal class ForStatementEmitter : BaseEmitter<ForStatement>
    {
        private readonly CodeSegment output;

        internal ForStatementEmitter(EmitterArguments arguments)
            : base(arguments)
        {
            output = arguments.Output;
        }

        internal override void Output(ForStatement node)
        {
            output.Add("for (");
            EmitterArguments.IsForStatementInitializerBusy = true;

            for (var i = 0; i < node.Initializers.Count; i++)
            {
                Emitter.ProcessNode(node.Initializers.ElementAt(i),EmitterArguments);
                if (i < node.Initializers.Count - 1)
                {
                    output.Add(",");
                }
            }
            output.Add(";");
            EmitterArguments.IsForStatementInitializerBusy = false;

            ExpressionEmitter.Output(node.Condition,EmitterArguments);
            output.Add(";");


            EmitterArguments.IsForStatementInitializerBusy = true;
            for (var i = 0; i < node.Iterators.Count; i++)
            {
                Emitter.ProcessNode(node.Iterators.ElementAt(i),EmitterArguments);
                if (i < node.Iterators.Count - 1) { output.Add(","); }
            }
            EmitterArguments.IsForStatementInitializerBusy = false;

            output.Add(") {");
            output.NewLine();

            output.IndentIncrease();
            Emitter.ProcessNode(node.EmbeddedStatement,EmitterArguments);
            output.IndentDecrease();
            output.AddLine("}");
        }

    }
}
