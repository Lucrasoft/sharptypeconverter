using Converter.Emitter.OutputFunctions.HelperFunctions;
using ICSharpCode.NRefactory.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter.Emitter.OutputFunctions
{
    class DelegateDeclarationEmitter : BaseEmitter<DelegateDeclaration>
    {
        readonly CodeSegment output;
        internal DelegateDeclarationEmitter(EmitterArguments arguments)
            : base(arguments)
        {
            output = arguments.Output;
        }

        internal override void Output(DelegateDeclaration node)
        {
            bool isPrivate = ((node.Modifiers & Modifiers.Private) == Modifiers.Private);

            if (isPrivate) { output.Add("private"); }

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
