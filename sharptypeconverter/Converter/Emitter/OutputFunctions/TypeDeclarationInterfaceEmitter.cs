using ICSharpCode.NRefactory.CSharp;

namespace Converter.Emitter.OutputFunctions
{
    internal static class TypeDeclarationInterfaceEmitter
    {
        internal static void Output(TypeDeclaration node, EmitterArguments arguments)
        {
            var output = arguments.Output;
            output.Add((node.Modifiers & Modifiers.Private) == Modifiers.Private ? "private" : "export");
            output.Add("interface");
            output.Add(node.Name);
            //Generic class defintion
            //Example 1 : public class Sample<T>
            //Example 2 : public class Sample<T> where T : stream
            TypeParametersEmitter.Output(node.TypeParameters, node.Constraints,arguments);
            BaseTypesEmitter.Output(node.BaseTypes, false,arguments);
            output.Add("{");
            output.NewLine();
            output.IndentIncrease();
            // members -> proces the node.Members.
            // this should be : fields, properties, constructors and methods
            foreach (var item in node.Members)
            {
                Emitter.ProcessNode(item,arguments);
            }
            output.IndentDecrease();
            output.NewLine();
            output.Add(" }");
            output.NewLine();
        }
    }
}
