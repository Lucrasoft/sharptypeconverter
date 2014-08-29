using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter.Emitter.OutputFunctions.RootEmitters
{
    internal static class PrimitiveValueEmitter
    {
        /// <summary>
        /// Adds the string used in typescript for the given primitive value to the given output code
        /// </summary>
        /// <param name="value"></param>
        /// <param name="output"></param>
        internal static void Output(Object value, CodeSegment output)
        {
            if (value == null)
            {
                output.AddWithoutSpace("null");
                return;
            }

            if (value is bool)
            {
                if ((bool)value)
                {
                    output.AddWithoutSpace("true");
                }
                else
                {
                    output.AddWithoutSpace("false");
                }
                return;
            }
            if (value is string)
            {
                output.AddWithoutSpace("\"" + (string)value + "\"");
                return;
            }
            output.AddWithoutSpace(value.ToString());
        }
    }
}
