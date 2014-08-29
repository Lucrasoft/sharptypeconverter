using ICSharpCode.NRefactory.CSharp;
using System;

namespace Converter.Emitter.OutputFunctions.RootEmitters
{
    internal static class BinaryOperatorEmitter
    {
        /// <summary>
        /// Adds the character(s) used in typescript for the given operatortype to the given output code
        /// </summary>
        /// <param name="operatorType"></param>
        /// <param name="output"></param>
        internal static void Output(BinaryOperatorType operatorType, CodeSegment output)
        {
            switch (operatorType)
            {
                case BinaryOperatorType.Add:
                    output.AddWithoutSpace("+");
                    break;
                case BinaryOperatorType.Any:
                    throw new NotImplementedException();
                case BinaryOperatorType.BitwiseAnd:
                    output.AddWithoutSpace("&");
                    break;
                case BinaryOperatorType.BitwiseOr:
                    output.AddWithoutSpace("|");
                    break;
                case BinaryOperatorType.ConditionalAnd:
                    output.AddWithoutSpace("&&");
                    break;
                case BinaryOperatorType.ConditionalOr:
                    output.AddWithoutSpace("||");
                    break;
                case BinaryOperatorType.Divide:
                    output.AddWithoutSpace("/");
                    break;
                case BinaryOperatorType.Equality:
                    output.AddWithoutSpace("==");
                    break;
                case BinaryOperatorType.ExclusiveOr:
                    output.AddWithoutSpace("^");
                    break;
                case BinaryOperatorType.GreaterThan:
                    output.AddWithoutSpace(">");
                    break;
                case BinaryOperatorType.GreaterThanOrEqual:
                    output.AddWithoutSpace(">=");
                    break;
                case BinaryOperatorType.InEquality:
                    output.AddWithoutSpace("!=");
                    break;
                case BinaryOperatorType.LessThan:
                    output.AddWithoutSpace("<");
                    break;
                case BinaryOperatorType.LessThanOrEqual:
                    output.AddWithoutSpace("<=");
                    break;
                case BinaryOperatorType.Modulus:
                    output.AddWithoutSpace("&");
                    break;
                case BinaryOperatorType.Multiply:
                    output.AddWithoutSpace("*");
                    break;
                case BinaryOperatorType.NullCoalescing:
                    output.AddWithoutSpace("??");
                    break;
                case BinaryOperatorType.ShiftLeft:
                    output.AddWithoutSpace("<<");
                    break;
                case BinaryOperatorType.ShiftRight:
                    output.AddWithoutSpace(">>");
                    break;
                case BinaryOperatorType.Subtract:
                    output.AddWithoutSpace("-");
                    break;
                default:
                    break;
            }
        }
    }
}
