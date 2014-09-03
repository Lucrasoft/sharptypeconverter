using ICSharpCode.NRefactory.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter.Emitter.OutputFunctions.RootEmitters
{
    internal static class UnaryOperatorEmitter
    {
        /// <summary>
        /// Adds the character(s) used in typescript for the given operatortype to the given output code
        /// </summary>
        /// <param name="operatorType"></param>
        /// <param name="output"></param>
        internal static void Output(UnaryOperatorType operatorType, CodeSegment output)
        {
            switch (operatorType)
            {
                case UnaryOperatorType.AddressOf:
                    output.AddWithoutSpace("AddressOf ");
                    break;
                case UnaryOperatorType.Decrement:
                    output.AddWithoutSpace("-");
                    break;
                case UnaryOperatorType.Dereference:
                    output.AddWithoutSpace("?");
                    break;
                case UnaryOperatorType.Increment:
                    output.AddWithoutSpace("+");
                    break;
                case UnaryOperatorType.Minus:
                    output.AddWithoutSpace("-");
                    break;
                case UnaryOperatorType.Not:
                    output.AddWithoutSpace("!");
                    break;
                case UnaryOperatorType.PostDecrement:
                    output.AddWithoutSpace("--");
                    break;
                case UnaryOperatorType.PostIncrement:
                    output.AddWithoutSpace("++");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
