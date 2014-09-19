using System.Linq;
using ICSharpCode.NRefactory.CSharp;
using Converter.TypeTree;
using Converter.Emitter.OutputFunctions.HelperFunctions;

namespace Converter.Emitter.OutputFunctions
{
    internal class OperatorDeclarationEmitter : BaseEmitter<OperatorDeclaration>
    {
        readonly CodeSegment output;
        readonly Tree typeTree;
        internal OperatorDeclarationEmitter(EmitterArguments arguments)
            : base(arguments)
        {
            output = arguments.Output;
            typeTree = arguments.TypeTree;
        }
        internal override void Output(OperatorDeclaration node)
        {
            output.Add((node.Modifiers & Modifiers.Private) == Modifiers.Private ? "private static" : "static");
            //Check if this method is overloaded in this Type.. 
            //If so : use the new name from the preprocess
            //syntaxTree.GetNodeAt(
            var operatorInfo = typeTree.ActiveType.Operators[node.OperatorType.ToString()];
            if (operatorInfo.isOverloaded)
            {
                var index = (from d in operatorInfo.declarations where (d.Line == node.StartLocation.Line) & (d.Column == node.StartLocation.Column) select d.Index).FirstOrDefault();
                var newname = node.OperatorType + "__" + index;
                output.AddWithoutSpace(newname);
            }
            else
            {
                output.AddWithoutSpace(node.OperatorType.ToString());
            }
            ParameterEmitter.Output(node.Parameters,EmitterArguments);
            output.Add(":" + AstTypeToStringConverter.Convert(node.ReturnType,EmitterArguments));

            output.AddLine("{");
            output.IndentIncrease();
            Emitter.ProcessNode(node.Body,EmitterArguments);
            output.IndentDecrease();
            output.AddLine("}");
        }
    }
}
