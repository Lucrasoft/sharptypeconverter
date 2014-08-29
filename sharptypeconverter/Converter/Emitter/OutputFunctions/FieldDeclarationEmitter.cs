using ICSharpCode.NRefactory.CSharp;
using Converter.Emitter.OutputFunctions.HelperFunctions;

namespace Converter.Emitter.OutputFunctions
{
    internal class FieldDeclarationEmitter : BaseEmitter<FieldDeclaration>
    {
        CodeSegment output;
        internal FieldDeclarationEmitter(EmitterArguments arguments)
            : base(arguments)
        {
            output = arguments.Output;
        }

        internal override void Output(FieldDeclaration node)
        {
            //Only check if it is declared as private;
            bool isPrivate = ((node.Modifiers & Modifiers.Private) == Modifiers.Private);
            bool isVar = false;
            if (node.ReturnType is SimpleType)
            {
                if (((SimpleType)node.ReturnType).Identifier == "var")
                {
                    isVar = true;
                }
            }
            foreach (VariableInitializer vi in node.Variables)
            {
                if (isPrivate) { output.Add("private"); }
                if (isVar)
                {
                    output.Add("var");
                    output.Add(vi.Name);
                }
                else
                {
                    output.AddWithoutSpace(vi.Name);
                    output.Add(":");
                    output.Add(AstTypeToStringConverter.Convert(node.ReturnType, EmitterArguments));
                }
                if (!vi.Initializer.IsNull)
                {
                    output.Add("=");
                    ExpressionEmitter.Output(vi.Initializer, EmitterArguments);
                }
                output.Add(";");
                output.NewLine();
            }
        }
    }
}
