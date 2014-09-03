using System.Linq;
using Converter.Emitter.OutputFunctions;
using ICSharpCode.NRefactory.CSharp;

namespace Converter.Emitter
{
    internal class SwitchStatementEmitter : BaseEmitter<SwitchStatement>
    {
        private readonly CodeSegment output;

        internal SwitchStatementEmitter(EmitterArguments arguments) : base(arguments)
        {
            output = arguments.Output;
        }

        internal override void Output(SwitchStatement node)
        {
            output.AddWithoutSpace("switch (");
            ExpressionEmitter.Output(node.Expression,EmitterArguments);
            output.AddLine(") {");
            output.IndentIncrease();
            foreach (SwitchSection item in node.SwitchSections)
            {
                output.Add("case ");
                for (int i = 0; i < item.CaseLabels.Count; i++)
                {
                    ExpressionEmitter.Output(item.CaseLabels.ElementAt(i).Expression,EmitterArguments);
                    if (i < item.CaseLabels.Count - 1) { output.Add(","); }
                }
                output.AddLine(":");
                output.IndentIncrease();
                foreach (var statement in item.Statements)
                {
                    Emitter.ProcessNode(statement,EmitterArguments);
                }
                output.IndentDecrease();
            }
            output.IndentDecrease();
            output.AddLine("}");
        }
    }
}
