using Converter.Emitter.OutputFunctions.HelperFunctions;
using Converter.TypeTree;
using ICSharpCode.NRefactory.CSharp;
using System.Linq;

namespace Converter.Emitter.OutputFunctions
{
    internal class MethodDeclarationEmitter : BaseEmitter<MethodDeclaration> {
        readonly CodeSegment output;
        readonly Tree typeTree;
        internal MethodDeclarationEmitter(EmitterArguments arguments) : base(arguments)
        {
            output = arguments.Output;
            typeTree = arguments.TypeTree;
        }

        internal override void Output(MethodDeclaration node)
        {
            bool isPrivate = ((node.Modifiers & Modifiers.Private) == Modifiers.Private);
            if (isPrivate) { output.Add("private"); }
            //Check if this method is overloaded in this Type.. 
            //If so : use the new name from the preprocess
            //syntaxTree.GetNodeAt(
            var methodInfo = typeTree.ActiveType.Methods[node.Name];
            if (methodInfo.isOverloaded)
            {
                var index = (from d in methodInfo.declarations where (d.Line == node.StartLocation.Line) & (d.Column == node.StartLocation.Column) select d.Index).FirstOrDefault();               
                var newname = string.Format("{0}__{1}", node.Name, index);
                output.AddWithoutSpace(newname);
            }
            else
            {
                output.AddWithoutSpace(node.Name);
            }
            //example :  public void MethodA<T1>() {} 
            TypeParametersEmitter.Output(node.TypeParameters, node.Constraints,EmitterArguments);
            ParameterEmitter.Output(node.Parameters,EmitterArguments);

            output.Add(":");
            output.Add(AstTypeToStringConverter.Convert(node.ReturnType,EmitterArguments));

            //If body is null, example when concerning an interface, skip the body
            if (node.Body.IsNull)
            {
                output.Add(";");
            }
            else
            {
                output.Add("{");
                output.NewLine();
                output.IndentIncrease();

                new BlockStatementEmitter(EmitterArguments).Output(node.Body);

                output.NewLine();
                output.IndentDecrease();
                output.Add("}");
            }

            output.NewLine();
        }
    }
}
