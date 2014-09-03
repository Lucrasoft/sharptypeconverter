using ICSharpCode.NRefactory.CSharp;
using System;

namespace Converter.Emitter.OutputFunctions.RootEmitters
{
    internal static class AssignmentOperatorEmitter
    {
        /// <summary>
        /// Adds the character(s) used in typescript for the given operatortype to the given output code
        /// </summary>
        /// <param name="operatorType"></param>
        /// <param name="output"></param>
        internal static void Output(AssignmentOperatorType operatorType, CodeSegment output)
        {
            switch (operatorType)
            {
                case AssignmentOperatorType.Add:
                    output.Add("+=");
                    break;
                case AssignmentOperatorType.Assign:
                    output.Add("=");
                    break;
                case AssignmentOperatorType.BitwiseAnd:
                    output.Add("&=");
                    break;
                case AssignmentOperatorType.BitwiseOr:
                    output.Add("|=");
                    break;
                case AssignmentOperatorType.Divide:
                    output.Add("/=");
                    break;
                case AssignmentOperatorType.ExclusiveOr:
                    output.Add("^=");
                    break;
                case AssignmentOperatorType.Modulus:
                    output.Add("%=");
                    break;
                case AssignmentOperatorType.Multiply:
                    output.Add("*=");
                    break;
                case AssignmentOperatorType.ShiftLeft:
                    output.Add("<<=");
                    break;
                case AssignmentOperatorType.ShiftRight:
                    output.Add(">>=");
                    break;
                case AssignmentOperatorType.Subtract:
                    output.Add("-=");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
