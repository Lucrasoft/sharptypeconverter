using ICSharpCode.NRefactory.CSharp;
using System.Linq;

namespace Converter.Emitter.OutputFunctions
{
    class ParameterEmitter
    {
        public static void Output(AstNodeCollection<ParameterDeclaration> parameters, EmitterArguments arguments)
        {
            arguments.Output.AddWithoutSpace("(");
            for (int i = 0; i < parameters.Count; i++)
            {
                var param = parameters.ElementAt(i);
                new ParameterDeclarationEmitter(arguments).Output(param);
                if (i < parameters.Count - 1)
                {
                    arguments.Output.Add(",");
                }
            }
            arguments.Output.Add(")");
        }
    }
}
