using System;

namespace Converter.Emitter.OutputFunctions.HelperFunctions
{
    internal static class TypeDeclarationExtractor
    {
        /// <summary>
        /// Searches through assembly to find given type in one of the given namespaces.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="arguments"></param>
        /// <returns>Full type name if found, otherwise the original typename</returns>
        internal static string ExtractFromNamespaces(string typeName, EmitterArguments arguments)
        {
            foreach (var nameSpace in arguments.ReferencedNamespaces)
            {
                var typeInfo = Type.GetType(nameSpace + "." + typeName);
                if (typeInfo != null)
                {
                    arguments.Output.AddTypeRequest(nameSpace, typeInfo);
                    return typeInfo.FullName;
                }
            }
            return typeName;
        }
    }
}
