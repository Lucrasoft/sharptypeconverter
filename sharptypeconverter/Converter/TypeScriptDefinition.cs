using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System;

namespace Converter
{
    public class TypeScriptDefinition
    {
        public List<string> Modules { get; set; }
        public string TypeName { get; set; }
        public List<string> Methods { get; set; }

        public string FullName
        {
            get { return Modules.Aggregate((current, next) => current + "." + next) + "." + TypeName; }
        }

        public TypeScriptDefinition()
        {
            Modules = new List<string>();
            Methods = new List<string>();
        }
        public TypeScriptDefinition(Type type)
        {
            var splitName = type.FullName.Split('.');
            Modules = splitName.Take(splitName.Length - 1).ToList();
            TypeName = splitName.Last();
            Methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public).Select(ConvertToString).ToList();
        }

        private static string ConvertToString(MethodInfo info)
        {
            var builder = new StringBuilder(info.Name);
            builder.Append("(");
            if (info.GetParameters().Any())
            {
                builder.Append(
                    info.GetParameters().Select(ConvertToString).Aggregate((current, next) => current + "," + next));
            }
            var filteredType = FilterTypeForLiteral(info.ReturnType.FullName);
            builder.Append(") : " + filteredType);
            if(filteredType.Equals(info.ReturnType.FullName))
            {
                
            }
            return builder.ToString();
        }
        private static string ConvertToString(ParameterInfo info)
        {
            var name = info.Name;
            var type = info.GetType().FullName;
            return name + " : " + type;
        }
        private static string FilterTypeForLiteral(string type)
        {
            switch (type)
            {
                case "System.string":
                    return "string";
                case "System.Boolean":
                    return "boolean";
                case "System.Int32":
                    return "number";
                case "System.Void" :
                    return "void";
                default:
                    return type;
            }
        }
        public override string ToString()
        {
            var typeDeclaration = new StringBuilder("declare ");
            for (var i = 0; i < Modules.Count(); i++)
            {
                typeDeclaration.AppendLine("module " + Modules.ElementAt(i) + "{");
            }
            typeDeclaration.AppendLine("class " + TypeName + "{");
            foreach (var method in Methods)
            {
                typeDeclaration.AppendLine(method);
            }
            typeDeclaration.AppendLine(" }");
            for (var i = 0; i < Modules.Count(); i++)
            {
                typeDeclaration.AppendLine("}");
            }
            return typeDeclaration.ToString();
        }
    }
}
