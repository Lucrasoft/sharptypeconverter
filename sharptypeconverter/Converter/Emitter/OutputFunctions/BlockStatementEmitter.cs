using ICSharpCode.NRefactory.CSharp;

namespace Converter.Emitter.OutputFunctions
{
    class BlockStatementEmitter: BaseEmitter<BlockStatement>
    {
        internal BlockStatementEmitter(EmitterArguments arguments) : base(arguments) { }

        internal override void Output(BlockStatement node)
        {
            foreach (var st in node.Statements)
            {
                Emitter.ProcessNode(st,EmitterArguments);
            }
        }
    }
}
