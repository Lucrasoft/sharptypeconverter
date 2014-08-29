using ICSharpCode.NRefactory.CSharp;
using System.Linq;

namespace Converter.Emitter.OutputFunctions
{
    class ArgumentsEmitter
    {
        readonly CodeSegment output;
        readonly EmitterArguments emitterArguments;

        internal ArgumentsEmitter(EmitterArguments arguments)
        {
            output = arguments.Output;
            emitterArguments = arguments;
        }

       internal void Output(AstNodeCollection<Expression> arguments)
        {
            for (int i = 0; i < arguments.Count; i++)
            {
                var expr = arguments.ElementAt(i);
                ExpressionEmitter.Output(expr,emitterArguments);
                if (i < arguments.Count - 1)
                {
                    output.Add(",");
                }
            }
        }
    }
}
