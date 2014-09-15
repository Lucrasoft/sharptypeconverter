using System;
using System.Linq;

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
                    var splitName = typeInfo.FullName.Split('.');
                    var modules = splitName.Take(splitName.Length -1).ToList();
                    arguments.Output.AddTypeDefinition(nameSpace,new TypeScriptDefinition()
                    {
                        Modules = modules,
                        TypeName = splitName.Last()
                    });
                    return typeInfo.FullName;
                }
            }
            return typeName;
        }
    }
}
