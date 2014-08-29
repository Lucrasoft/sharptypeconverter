using System;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;

namespace Converter.Emitter.OutputFunctions
{
    internal class UsingStatementEmitter : BaseEmitter<UsingStatement>
    {
        private readonly CodeSegment output;
        internal UsingStatementEmitter(EmitterArguments arguments) : base(arguments)
        {
            output = arguments.Output;
        }
        /// <summary>
        ///  Using statement gets translated into a try/final loop
        ///  http://www.codeproject.com/Articles/6564/Understanding-the-using-statement-in-C
        /// </summary>
        /// <param name="node"></param>
        internal override void Output(UsingStatement node)
        {
            var varName = UsingStatementExtractResource(node.ResourceAcquisition);
            Emitter.ProcessNode(node.ResourceAcquisition, EmitterArguments);
            output.AddLine("try {");

            output.IndentIncrease();
            Emitter.ProcessNode(node.EmbeddedStatement, EmitterArguments);
            output.IndentDecrease();

            output.AddLine("} finally {");

            output.IndentIncrease();
            output.AddLine("if (" + varName + ") { " + varName + ".Dispose(); }");
            output.IndentDecrease();

            output.AddLine("}");
        }
        private string UsingStatementExtractResource(AstNode node)
        {
            if (node is IdentifierExpression)
            {
                return ((IdentifierExpression)node).Identifier;
            }
            if (node is VariableDeclarationStatement)
            {
                return ((VariableDeclarationStatement)node).Variables.ElementAt(0).Name;
            }
            throw new NotImplementedException();
        }
    }
}