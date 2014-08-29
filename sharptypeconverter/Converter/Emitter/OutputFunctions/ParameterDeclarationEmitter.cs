using Converter.Emitter.OutputFunctions.HelperFunctions;
using ICSharpCode.NRefactory.CSharp;

namespace Converter.Emitter.OutputFunctions
{
    internal class ParameterDeclarationEmitter: BaseEmitter<ParameterDeclaration>
    {
        readonly EmitterArguments arguments;
        readonly CodeSegment output;

        internal ParameterDeclarationEmitter(EmitterArguments arguments):base(arguments)
        {
            this.arguments = arguments;
            output = arguments.Output;
        }

        internal override void Output(ParameterDeclaration node)
        {
            //example :  void SomeMethod(params string[] options )
            if (node.ParameterModifier == ParameterModifier.Params)
            {
                output.AddWithoutSpace("..." + node.Name);
                output.Add(":");
                output.AddWithoutSpace(AstTypeToStringConverter.Convert(node.Type, arguments));
            }
            else
                if ((node.ParameterModifier == ParameterModifier.Ref) || (node.ParameterModifier == ParameterModifier.Out))
                {
                    output.AddWithoutSpace(node.Name);
                    output.Add(":");
                    output.AddWithoutSpace(" { value : " + AstTypeToStringConverter.Convert(node.Type, arguments) + "}");
                }
                else
                {
                    output.AddWithoutSpace(node.Name);
                    output.Add(":");
                    output.AddWithoutSpace(AstTypeToStringConverter.Convert(node.Type, arguments));
                }
            if (!node.DefaultExpression.IsNull)
            {
                output.Add("=");
                ExpressionEmitter.Output(node.DefaultExpression,arguments);
            }
        }
    }
}
