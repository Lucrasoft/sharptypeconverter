using System.Linq;
using Converter.Emitter.OutputFunctions.HelperFunctions;
using ICSharpCode.NRefactory.CSharp;

namespace Converter.Emitter.OutputFunctions
{
    internal class VariableDeclarationStatementEmitter : BaseEmitter<VariableDeclarationStatement>
    {
        private readonly CodeSegment output;
        internal VariableDeclarationStatementEmitter(EmitterArguments arguments) : base(arguments)
        {
            output = arguments.Output;
        }
        internal override void Output(VariableDeclarationStatement node)
        {
            var isVar = (node.Type.ToString() == "var");
            output.Add("var");
            for (int i = 0; i < node.Variables.Count; i++)
            {
                VariableInitializer vi = node.Variables.ElementAt(i);
                
                output.Add(vi.Name);
                if (!isVar)
                {
                    output.Add(":");
                    output.Add(AstTypeToStringConverter.Convert(node.Type,EmitterArguments));
                }

                if (!vi.Initializer.IsNull)
                {
                    output.Add("=");
                    ExpressionEmitter.Output(vi.Initializer,EmitterArguments);
                }
                if (i < node.Variables.Count - 1)
                {
                    output.Add(",");
                }
            }
            if (!EmitterArguments.IsForStatementInitializerBusy)
            {
                output.Add(";");
                output.NewLine();
            }
        }
    }
}
