using Converter.TypeTree;
using ICSharpCode.NRefactory.CSharp;

namespace Converter.Emitter.OutputFunctions
{
    internal static class MethodOverloadDispatchEmitter
    {
        internal static void Output(TIMethod method, EmitterArguments arguments)
        {
            var output = arguments.Output;
            var syntaxTree = arguments.SyntaxTree;
             var tempd = method.declarations[0];
            var refMethod = syntaxTree.GetNodeAt<MethodDeclaration>(tempd.Line, tempd.Column);

            output.Comment("Overloaded method dispatch function");

            if ((refMethod.Modifiers & Modifiers.Private) == Modifiers.Private)
            {
                output.Add("private");
            }
            else
            {
                output.Add("public");
            }
            output.Add(refMethod.Name);
            TypeParametersEmitter.Output(refMethod.TypeParameters, refMethod.Constraints,arguments);
            output.AddWithoutSpace("(...params : any[]) {");
            output.NewLine();
            output.IndentIncrease();

            //first attempt
            var c = 0;
            foreach (var d in method.declarations)
            {
                var item = syntaxTree.GetNodeAt<MethodDeclaration>(d.Line, d.Column);

                output.AddLine("if (params.length==" + item.Parameters.Count + ") {");
                output.IndentIncrease();
                output.Add(string.Format("this.{0}__{1}(", refMethod.Name, c));
                for (int i = 0; i < item.Parameters.Count; i++)
                {
                    output.Add("params[" + i + "]");
                    if (i < item.Parameters.Count - 1)
                    {
                        output.Add(",");
                    }
                }
                output.Add(");");
                output.NewLine();
                output.Add("return;");
                output.IndentDecrease();
                output.AddLine("}");
                c++;
            }
            output.IndentDecrease();
            output.AddLine("}");
        }
    }
}
