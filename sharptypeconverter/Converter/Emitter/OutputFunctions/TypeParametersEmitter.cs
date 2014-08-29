using Converter.Emitter.OutputFunctions.HelperFunctions;
using ICSharpCode.NRefactory.CSharp;
using System.Linq;

namespace Converter.Emitter.OutputFunctions
{
    internal class TypeParametersEmitter{

        internal static void Output(AstNodeCollection<TypeParameterDeclaration> typeParameters, AstNodeCollection<Constraint> constraints, EmitterArguments arguments)
        {
            var output = arguments.Output;
            if (typeParameters.Count > 0)
            {
                output.AddWithoutSpace("<");
                for (int i = 0; i < typeParameters.Count; i++)
                {
                    var paramItem = typeParameters.ElementAt(i);
                    var param = paramItem.Name;
                    output.AddWithoutSpace(param);
                    //Are there Constraints on the TypeParameters like : where T : object
                    if (constraints.Count > 0)
                    {
                        var constraint = (from x in constraints where x.TypeParameter.Identifier == param select x).FirstOrDefault();
                        if (constraint != null)
                        {
                            output.Add(" extends");
                            for (var j = 0; j < constraint.BaseTypes.Count; j++)
                            {
                                var baseType = constraint.BaseTypes.ElementAt(j);
                                output.AddWithoutSpace(AstTypeToStringConverter.Convert(baseType,arguments));
                                if (j < constraint.BaseTypes.Count - 1)
                                {
                                    output.Add(",");
                                }
                            }
                        }
                    }
                    if (i < typeParameters.Count - 1)
                    {
                        output.Add(",");
                    }
                }
                output.Add(">");
            }
        }
    }
}
