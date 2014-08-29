using System;
using ICSharpCode.NRefactory.CSharp;

namespace Converter.Emitter
{
    internal static class EnumTypeDeclarationEmitter
    {
        internal static void Output(TypeDeclaration node, EmitterArguments arguments)
        {
            var output = arguments.Output;
            output.Add((node.Modifiers & Modifiers.Private) == Modifiers.Private ? "private" : "export");
            output.Add("enum");
            output.Add(node.Name);
            output.Add("{");
            output.NewLine();
            output.IndentIncrease();
            // members -> proces the node.Members.
            // this should be : EnumMemberDeclartion
            var c = 0;
            foreach (var item in node.Members)
            {
                Emitter.ProcessNode(item,arguments);
                c++;
                if (c < node.Members.Count)
                {
                    output.Add(",");
                    output.NewLine();
                }
            }
            output.NewLine();
            output.IndentDecrease();
            output.NewLine();
            output.Add(" }");
            output.NewLine();
            //register interface
            output.Add(String.Format("System.Type.registerEnum({0},'{1}');", node.Name, node.Name));
        }
    }
}
