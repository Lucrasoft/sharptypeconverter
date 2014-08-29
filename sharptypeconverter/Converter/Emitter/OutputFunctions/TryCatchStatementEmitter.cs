using ICSharpCode.NRefactory.CSharp;

namespace Converter.Emitter.OutputFunctions
{
    internal class TryCatchStatementEmitter : BaseEmitter<TryCatchStatement>
    {
        private readonly CodeSegment output;
        internal TryCatchStatementEmitter(EmitterArguments arguments) : base(arguments)
        {
            output = arguments.Output;
        }
        internal override void Output(TryCatchStatement node)
        {
            output.AddLine("try {");
            output.IndentIncrease();

            Emitter.ProcessNode(node.TryBlock,EmitterArguments);

            output.IndentDecrease();
            output.AddLine("}");
            if (node.CatchClauses.Count > 0)
            {
                output.AddLine("catch (ex) {");
                output.IndentIncrease();

                foreach (CatchClause item in node.CatchClauses)
                {
                    //TODO : TS wil only catch an ex. use the typing of Ex to discover which original Catch call it through some "dispath" thing 
                    output.Comment("CatchClause not handled at the moment..");
                    output.AddLine(item.ToString());
                }
                output.IndentDecrease();
                output.AddLine("}");
            }
            if (!node.FinallyBlock.IsNull)
            {
                output.AddLine("finally {");
                Emitter.ProcessNode(node.FinallyBlock,EmitterArguments);
                output.AddLine("}");
            }
        }
    }
}
