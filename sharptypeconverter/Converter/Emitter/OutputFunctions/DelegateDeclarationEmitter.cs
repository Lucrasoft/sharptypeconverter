using Converter.Emitter.OutputFunctions.HelperFunctions;
using ICSharpCode.NRefactory.CSharp;

namespace Converter.Emitter.OutputFunctions
{
    class DelegateDeclarationEmitter : BaseEmitter<DelegateDeclaration>
    {
        private readonly CodeSegment output;
        private readonly string accessModifier;
        internal DelegateDeclarationEmitter(EmitterArguments arguments)
            : base(arguments)
        {
            accessModifier = null;
            output = arguments.Output;
        }
        internal DelegateDeclarationEmitter(EmitterArguments arguments,string modifier)
            : base(arguments)
        {
            accessModifier = modifier;
            output = arguments.Output;
        }

        internal override void Output(DelegateDeclaration node)
        {
            bool isPrivate = ((node.Modifiers & Modifiers.Private) == Modifiers.Private);

            if (!string.IsNullOrEmpty(accessModifier))
            {
                output.Add(accessModifier + " ");
            }

            //Check if this method is overloaded in this Type.. 
            //If so : use the new name from the preprocess
            output.Add("interface");
            output.AddWithoutSpace(node.Name);

            //example :  public void MethodA<T1>() {} 
            TypeParametersEmitter.Output(node.TypeParameters, node.Constraints,EmitterArguments);
            output.AddLine("{");
            output.IndentIncrease();
            ParameterEmitter.Output(node.Parameters,EmitterArguments);
            output.Add(":");
            output.Add(AstTypeToStringConverter.Convert(node.ReturnType,EmitterArguments));
            output.IndentDecrease();
            output.AddLine("}");
        }
    }
}
